using Microsoft.AspNetCore.Mvc;
using WebApp.Members.Models;

namespace WebApp.Members;

public class MembersController : Controller
{
    private readonly object _logger;
    private readonly IMembersService _membersService;

    public MembersController(
            ILogger<MembersController> logger,
            IMembersService membersService) {
        this._logger = logger;
        this._membersService = membersService;
    }

    public async Task<ActionResult> Index() {
        List<Member> members = await this._membersService.GetMembersAsync();

        MembersViewModel model = new MembersViewModel {
            Members = members.Select(m => new MembersViewModel.Member {
                Id = m.Id, 
                Name = m.Name, 
                PhoneNumber = m.PhoneNumber, 
                Email = m.Email, 
                Section = m.Section.ToString(),
                Roles = m.Roles.Select(r => r.ToString()) 
            }) 
        };
        return View("MemberList", model);
    }

    [HttpGet]
    public ActionResult New()
    {
        CreateMemberViewModel newMember = new CreateMemberViewModel();
        return View("NewMember", newMember);
    }
    
    [HttpPost]
    public async Task<ActionResult> New(CreateMemberViewModel model) {
        CreateMemberModel newMember = model.ToModel();
        Member member = await this._membersService.CreateMemberAsync(newMember);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id) {
        Member? member = await this._membersService.GetMemberByIdAsync(id);
        if (member is null) {
            return NotFound();
        }
        EditMemberViewModel model = EditMemberViewModel.FromMember(member);
        return View("EditMember", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditMemberViewModel model) {
        Member member = model.ToMember();
        Member? editedMember = await this._membersService.EditMemberAsync(member);
        if (editedMember is null) {
            return NotFound();
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id) {
        Member? member = await this._membersService.DeleteMemberAsync(id);
        if (member is null) {
            return NotFound();
        }
        return RedirectToAction("Index");
    }
}
