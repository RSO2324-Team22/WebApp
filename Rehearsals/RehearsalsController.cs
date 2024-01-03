using Microsoft.AspNetCore.Mvc;

namespace WebApp.Rehearsals;

public class RehearsalsController : Controller
{
    // GET: RehearsalsController
    public ActionResult Index()
    {
        return View();
    }

}
