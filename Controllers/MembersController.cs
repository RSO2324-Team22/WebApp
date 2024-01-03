using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace MyApp.Namespace
{
    public class MembersController : Controller
    {
        // GET: MembersController
        public ActionResult Index() {
            return View();
        }
        
        [HttpGet]
        public ActionResult Edit(int id) {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, EditMemberViewModel model) {
            return View();
        }
    }
}
