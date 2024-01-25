using Microsoft.AspNetCore.Mvc;
using WebApp.ConcertAttendances.Models;
using WebApp.Concerts;

namespace WebApp.ConcertAttendances;

[Route("ConcertAttendance")]
public class ConcertAttendancesController : Controller
{
    private ILogger<ConcertAttendancesController> _logger;
    private readonly IConcertsService _concertsService;
    private readonly IConcertAttendanceService _attendanceService;

    public ConcertAttendancesController(
            ILogger<ConcertAttendancesController> logger,
            IConcertsService concertsService,
            IConcertAttendanceService attendanceService) {
        this._logger = logger;
        this._concertsService = concertsService;
        this._attendanceService = attendanceService;
    }
    
    [HttpGet]
    [Route("Index/{id}")]
    public async Task<ActionResult> Index(int id) {
        Concert? concert = await this._concertsService.GetConcertByIdAsync(id);
        if (concert is null) {
            return NotFound();
        }

        List<ConcertAttendance> attendances = 
            await this._attendanceService.GetConcertAttendanceAsync(concert);

        ConcertAttendanceViewModel model = new ConcertAttendanceViewModel {
            ConcertId = id,
            ConcertTitle = concert.Title,
            Attendances = attendances.Select(a => new ConcertAttendanceModel {
                Id = a.Id,
                Member = a.Member,
                IsPresent = a.IsPresent,
                ReasonForAbsence = a.ReasonForAbsence ?? ""
            }).ToList()
        };

        return View("ConcertAttendance", model);
    }

    [HttpPost]
    [Route("Edit/{id}")]
    public async Task<ActionResult> Edit(int id, ConcertAttendanceViewModel model) {
        Concert? concert = await this._concertsService.GetConcertByIdAsync(id);
        if (concert is null) {
            return NotFound();
        }

        List<ConcertAttendance> attendances = 
            await this._attendanceService.GetConcertAttendanceAsync(concert);

        List<ConcertAttendance> editedAttendances = 
            await this._attendanceService.EditConcertAttendanceAsync(concert, model.Attendances);

        return RedirectToAction("Index", new {concertId = id});
    }
}
