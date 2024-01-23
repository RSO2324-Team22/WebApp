using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Razor;
using OpenTelemetry.Metrics;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Events;
using Serilog.HttpClient.Extensions;
using WebApp;
using WebApp.Concerts;
using WebApp.Members;
using WebApp.Rehearsals;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureBuilder(builder);

        var app = builder.Build();
        ConfigureApplication(app);

        app.Run();
    }

    private static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddHeaderPropagation(options => {
            options.Headers.Add("X-Correlation-Id");
        });
        
        ConfigureMvc(builder);
        ConfigureHttpClients(builder);
        ConfigureLogging(builder);
        ConfigureMetrics(builder);
    }

    private static void ConfigureMvc(WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new AppViewLocator());
        });
        builder.Services.AddHttpContextAccessor();
    }

    private static void ConfigureHttpClients(WebApplicationBuilder builder) {
        builder.Services.AddHeaderPropagation(options => {
            options.Headers.Add("X-Correlation-Id");
        });

        IAsyncPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        IAsyncPolicy<HttpResponseMessage> circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        string? members_service_url = builder.Configuration["MEMBERS_SERVICE_URL"];
        builder.Services.AddHttpClient<IMembersService, MembersService>(client => {
            client.BaseAddress = new Uri($"{members_service_url}/member");
        }).AddHeaderPropagation()
          .AddPolicyHandler(retryPolicy)
          .AddPolicyHandler(circuitBreakerPolicy)
          .LogRequestResponse();
        
        string? planning_service_url = builder.Configuration["PLANNING_SERVICE_URL"];
        builder.Services.AddHttpClient<IConcertsService, ConcertsService>(client => {
            client.BaseAddress = new Uri($"{planning_service_url}/concert");
        }).AddHeaderPropagation()
          .AddPolicyHandler(retryPolicy)
          .AddPolicyHandler(circuitBreakerPolicy)
          .LogRequestResponse();


        builder.Services.AddHttpClient<IRehearsalsService, RehearsalsService>(client => {
            client.BaseAddress = new Uri($"{planning_service_url}/rehearsal");
        }).AddHeaderPropagation()
          .AddPolicyHandler(retryPolicy)
          .AddPolicyHandler(circuitBreakerPolicy)
          .LogRequestResponse();

        string? attendance_service_url = builder.Configuration["ATTENDANCE_SERVICE_URL"];
    }

    private static void ConfigureLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) => {
            config.ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithCorrelationIdHeader("X-Correlation-Id");
        });
    }

    private static void ConfigureMetrics(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(builder => {
                builder.AddPrometheusExporter();

                builder.AddMeter(
                    "Microsoft.AspNetCore.Hosting",
                    "Microsoft.AspNetCore.Server.Kestrel");
            });
    }

    private static void ConfigureApplication(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        ConfigureWebApplication(app);
        ConfigureApplicationLogging(app);
    }

    private static void ConfigureWebApplication(WebApplication app)
    {
        string app_base = app.Configuration["APP_BASE"] ?? "/";
        app.UsePathBase(app_base);

        string correlationIdKey = "X-Correlation-Id";
        app.Use(async (context, next) => {
            context.Response.OnStarting(() => {
                if (context.Response.Headers[correlationIdKey] == "") {
                    string correlationId = Guid.NewGuid().ToString();
                    context.Response.Headers[correlationIdKey] = correlationId;
                }
                return Task.CompletedTask;
            });

            await next.Invoke(context);
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.UseHeaderPropagation();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Members}/{action=Index}/{id?}");
    }

    private static void ConfigureApplicationLogging(WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            // Customize the message template
            options.MessageTemplate = "Handled {RequestPath}";

            // Emit debug-level events instead of the defaults
            options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

            // Attach additional properties to the request completion event
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            };
        });
    }
}
