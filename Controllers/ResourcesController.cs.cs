using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ResourcesController : Controller
    {
        // GET: ResourcesController.cs
        public ActionResult Index()
        {
            return View();
        }

    }
}