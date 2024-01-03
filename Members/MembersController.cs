using Microsoft.AspNetCore.Mvc;

namespace WebApp.Members;

public class MembersController : Controller
{
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
