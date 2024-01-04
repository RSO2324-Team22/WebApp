using Microsoft.AspNetCore.Mvc.Razor;
using WebApp;
using WebApp.Concerts;
using WebApp.Members;
using WebApp.Rehearsals;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMembersService, MembersService>();
builder.Services.AddScoped<IConcertService, ConcertService>();
builder.Services.AddScoped<IRehearsalService, RehearsalService>();

builder.Services.Configure<RazorViewEngineOptions>(options => {
    options.ViewLocationExpanders.Add(new AppViewLocator());      
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Members}/{action=Index}/{id?}");

app.Run();
