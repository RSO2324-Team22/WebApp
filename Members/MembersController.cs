using Microsoft.AspNetCore.Mvc;

namespace WebApp.Members;

public class MembersController : Controller
{
    private readonly object _logger;
    private readonly MembersService _membersService;

    public MembersController(
            ILogger<MembersController> logger,
            MembersService membersService) {
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
        return View(model);
    }

    [HttpGet]
    public ActionResult New() {
        return View();
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
    public async Task<ActionResult> Edit(int id, EditMemberViewModel model) {
        Member member = model.ToMember();
        Member editedMember = await this._membersService.EditMemberAsync(member);
        return View(EditMemberViewModel.FromMember(editedMember));
    }

    [HttpPost]
    public async Task<ActionResult> Delete(int id) {
        await this._membersService.DeleteMemberAsync(id); 
        return RedirectToAction("Index");
    }
}
