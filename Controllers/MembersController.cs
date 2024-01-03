using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class MembersController : Controller
    {
        // GET: MembersController
        public ActionResult Index() {
            return View();
        }

        public ActionResult Edit(int id) {
            return View();
        }
    }
}
