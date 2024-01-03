using Microsoft.AspNetCore.Mvc;

namespace WebApp.Concerts;

public class ConcertsController : Controller
{
    // GET: ConcertsController
    public ActionResult Index()
    {
        return View();
    }
}
