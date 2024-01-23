using Microsoft.AspNetCore.Mvc;
using WebApp.Rehearsals.Models;

namespace WebApp.Rehearsals;

public class RehearsalsController : Controller
{
    private readonly object _logger;
    private readonly IRehearsalsService _rehearsalService;

    public RehearsalsController(
        ILogger<RehearsalsController> logger,
        IRehearsalsService rehearsalService) {
        this._logger = logger;
        this._rehearsalService = rehearsalService;
    }
    
    public async Task<ActionResult> Index() {
        List<Rehearsal> concerts = await this._rehearsalService.GetRehearsalsAsync();

        RehearsalsViewModel model = new RehearsalsViewModel {
            Rehearsals = concerts.Select(c => new RehearsalsViewModel.Rehearsal {
                Id = c.Id,
                Title = c.Title,
                Location = c.Location,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                Notes = c.Notes,
                Status = c.Status
            }) 
        };
        return View("RehearsalList", model);
    }

    [HttpGet]
    public ActionResult New()
    {
        CreateRehearsalViewModel newRehearsal = new CreateRehearsalViewModel();
        return View("NewRehearsal", newRehearsal);
    }
    
    [HttpPost]
    public async Task<ActionResult> New(CreateRehearsalViewModel model) {
        CreateRehearsalModel newRehearsal = model.ToNewRehearsal();
        Rehearsal? rehearsal = await this._rehearsalService.CreateRehearsalAsync(newRehearsal);
        if (rehearsal is null) {
            return NotFound();
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id) {
        Rehearsal? rehearsal = await this._rehearsalService.GetRehearsalByIdAsync(id);
        if (rehearsal is null) {
            return NotFound();
        }

        EditRehearsalViewModel model = EditRehearsalViewModel.FromRehearsal(rehearsal);
        return View("EditRehearsal", model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRehearsalViewModel model) {
        Rehearsal rehearsal = model.ToRehearsal();
        Rehearsal? editedRehearsal = await this._rehearsalService.EditRehearsalAsync(rehearsal);
        if (editedRehearsal is null) {
            return NotFound();
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Delete(int id) {
        Rehearsal? rehearsal = await this._rehearsalService.DeleteRehearsalAsync(id); 
        if (rehearsal is null) {
            return NotFound();
        }

        return RedirectToAction("Index");
    }
}
