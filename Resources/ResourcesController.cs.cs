using Microsoft.AspNetCore.Mvc;

namespace WebApp.Resources;

public class ResourcesController : Controller
{
    // GET: ResourcesController.cs
    public ActionResult Index()
    {
        return View();
    }

}
