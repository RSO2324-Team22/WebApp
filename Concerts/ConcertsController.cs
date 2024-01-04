using Microsoft.AspNetCore.Mvc;
using WebApp.Concerts.Models;
using WebApp.Concerts.Views;
using WebApp.Shared;

namespace WebApp.Concerts;

public class ConcertsController : Controller
{
    private readonly object _logger;
    private readonly IConcertService _concertService;

    public ConcertsController(
        ILogger<ConcertsController> logger,
        IConcertService concertService) {
        this._logger = logger;
        this._concertService = concertService;
    }
    
    public async Task<ActionResult> ConcertList() {
        List<Concert> concerts = await this._concertService.GetConcertsAsync();

        ConcertViewModel model = new ConcertViewModel {
            Concerts = concerts.Select(c => new ConcertViewModel.Concert {
                Id = c.id,
                Title = c.title,
                Location = new Location()
                {
                    Latitude = c.location.latitude,
                    Longitude = c.location.longitude,
                },
                MeetupTime = c.meetupTime,
                SoundCheckTime = c.soundCheckTime,
                StartTime = c.startTime,
                ExpectedEndTime = c.expectedEndTime,
                Notes = c.notes,
                Status = c.status
            }) 
        };
        return View(model);
    }

    [HttpGet]
    public ActionResult NewConcert()
    {
        CreateConcertViewModel newConcert = new CreateConcertViewModel();
        return View(newConcert);
    }
    
    [HttpPost]
    public async Task<ActionResult> NewConcert(CreateConcertViewModel model) {
        NewConcert newConcert = model.ToNewConcert();
        Concert concert = await this._concertService.CreateConcertAsync(newConcert);
        return RedirectToAction("ConcertList");
    }

    [HttpGet]
    public async Task<ActionResult> EditConcert(int id) {
        Concert concert = await this._concertService.GetConcertByIdAsync(id);
        EditConcertViewModel model = EditConcertViewModel.FromConcert(concert);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditConcert(int id, EditConcertViewModel model) {
        Concert member = model.ToConcert();
        Concert editedConcert = await this._concertService.EditConcertAsync(member);
        return RedirectToAction("ConcertList");
    }

    [HttpGet]
    public async Task<ActionResult> DeleteConcert(int id) {
        await this._concertService.DeleteConcertAsync(id); 
        return RedirectToAction("ConcertList");
    }
}
