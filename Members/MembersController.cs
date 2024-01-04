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
                Id = m.id, 
                Name = m.name, 
                PhoneNumber = m.phoneNumber, 
                Email = m.email, 
                Section = m.section.ToString(),
                Roles = m.roles.Select(r => r.ToString()) 
            }) 
        };
        return View(model);
    }

    [HttpGet]
    public ActionResult New()
    {
        CreateMemberViewModel newMember = new CreateMemberViewModel();
        return View(newMember);
    }
    
    [HttpPost]
    public async Task<ActionResult> New(CreateMemberViewModel model) {
        NewMember newMember = model.ToNewMember();
        Member member = await this._membersService.CreateMemberAsync(newMember);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id) {
        Member member = await this._membersService.GetMemberByIdAsync(id);
        EditMemberViewModel model = EditMemberViewModel.FromMember(member);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,Email,Section,SingerRole,CouncilRole,ConductorRole")] EditMemberViewModel model) {
        Member member = model.ToMember();
        Member editedMember = await this._membersService.EditMemberAsync(member);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id) {
        await this._membersService.DeleteMemberAsync(id); 
        return RedirectToAction("Index");
    }
}
