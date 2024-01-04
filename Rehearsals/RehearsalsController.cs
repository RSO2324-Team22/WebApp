using Microsoft.AspNetCore.Mvc;
using WebApp.Concerts.Models;
using WebApp.Concerts.Views;
using WebApp.Rehearsals;
using WebApp.Rehearsals.Models;
using WebApp.Rehearsals.Views;
using WebApp.Shared;

namespace WebApp.Rehearsals;

public class RehearsalsController : Controller
{
    private readonly object _logger;
    private readonly IRehearsalService _rehearsalService;

    public RehearsalsController(
        ILogger<RehearsalsController> logger,
        IRehearsalService rehearsalService) {
        this._logger = logger;
        this._rehearsalService = rehearsalService;
    }
    
    public async Task<ActionResult> RehearsalList() {
        List<Rehearsal> concerts = await this._rehearsalService.GetRehearsalsAsync();

        RehearsalViewModel model = new RehearsalViewModel {
            Rehearsals = concerts.Select(c => new RehearsalViewModel.Rehearsal {
                Id = c.id,
                Title = c.title,
                Location = new Location()
                {
                    Latitude = c.location.latitude,
                    Longitude = c.location.longitude,
                },
                StartTime = c.startTime,
                EndTime = c.endTime,
                Notes = c.notes,
                Status = c.status
            }) 
        };
        return View(model);
    }

    [HttpGet]
    public ActionResult NewRehearsal()
    {
        CreateRehearsalViewModel newConcert = new CreateRehearsalViewModel();
        return View(newConcert);
    }
    
    [HttpPost]
    public async Task<ActionResult> NewRehearsal(CreateRehearsalViewModel model) {
        NewRehearsal newRehearsal = model.ToNewRehearsal();
        Rehearsal concert = await this._rehearsalService.CreateRehearsalAsync(newRehearsal);
        return RedirectToAction("RehearsalList");
    }

    [HttpGet]
    public async Task<ActionResult> EditRehearsal(int id) {
        Rehearsal rehearsal = await this._rehearsalService.GetRehearsalByIdAsync(id);
        EditRehearsalViewModel model = EditRehearsalViewModel.FromRehearsal(rehearsal);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRehearsal(int id, EditRehearsalViewModel model) {
        Rehearsal rehearsal = model.ToRehearsal();
        Rehearsal editedRehearsal = await this._rehearsalService.EditRehearsalAsync(rehearsal);
        return RedirectToAction("RehearsalList");
    }

    [HttpGet]
    public async Task<ActionResult> DeleteRehearsal(int id) {
        await this._rehearsalService.DeleteRehearsalAsync(id); 
        return RedirectToAction("RehearsalList");
    }
}
