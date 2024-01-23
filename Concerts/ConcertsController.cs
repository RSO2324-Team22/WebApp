using Microsoft.AspNetCore.Mvc;
using WebApp.Concerts.Models;
namespace WebApp.Concerts;

public class ConcertsController : Controller
{
    private readonly object _logger;
    private readonly IConcertsService _concertService;

    public ConcertsController(
        ILogger<ConcertsController> logger,
        IConcertsService concertService) {
        this._logger = logger;
        this._concertService = concertService;
    }
    
    public async Task<ActionResult> Index() {
        List<Concert> concerts = await this._concertService.GetConcertsAsync();

        ConcertsViewModel model = new ConcertsViewModel {
            Concerts = concerts.Select(c => new ConcertsViewModel.Concert {
                Id = c.Id,
                Title = c.Title,
                Location = c.Location,
                MeetupTime = c.MeetupTime,
                SoundCheckTime = c.SoundCheckTime,
                StartTime = c.StartTime,
                ExpectedEndTime = c.ExpectedEndTime,
                Notes = c.Notes,
                Status = c.Status
            }) 
        };
        return View("ConcertList", model);
    }

    [HttpGet]
    public ActionResult New()
    {
        CreateConcertViewModel newConcert = new CreateConcertViewModel();
        return View("NewConcert", newConcert);
    }
    
    [HttpPost]
    public async Task<ActionResult> New(CreateConcertViewModel model) {
        CreateConcertModel newConcert = model.ToNewConcert();
        Concert concert = await this._concertService.CreateConcertAsync(newConcert);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> Edit(int id) {
        Concert concert = await this._concertService.GetConcertByIdAsync(id);
        EditConcertViewModel model = EditConcertViewModel.FromConcert(concert);
        return View("EditConcert", model);
    }

    [HttpPost]
    public async Task<IActionResult> EditConcert(int id, EditConcertViewModel model) {
        Concert member = model.ToConcert();
        Concert editedConcert = await this._concertService.EditConcertAsync(member);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<ActionResult> DeleteConcert(int id) {
        await this._concertService.DeleteConcertAsync(id); 
        return RedirectToAction("Index");
    }
}
