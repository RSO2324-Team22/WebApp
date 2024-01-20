using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApp;

public class ErrorController : Controller {
    private readonly ILogger<ErrorController> _logger;
    private readonly HttpContext? _httpContext;

    public ErrorController(
            ILogger<ErrorController> logger,
            IHttpContextAccessor httpContextAccessor) {
        this._logger = logger;
        this._httpContext = httpContextAccessor.HttpContext;
    }

    public ActionResult Index() {
        var exceptionHandlerPathFeature = 
            this._httpContext?.Features.Get<IExceptionHandlerPathFeature>();

        var exception = exceptionHandlerPathFeature?.Error;
        if (exception is not null) {
            this._httpContext?.RequestServices
                .GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>()
                .LogError(exception, "An error has occurred while processing request");
        }

        return View("Error");
    }
}
