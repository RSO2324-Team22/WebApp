using Microsoft.AspNetCore.Mvc;
using WebApp.RehearsalAttendances.Models;
using WebApp.Rehearsals;

namespace WebApp.RehearsalAttendances;

[Route("RehearsalAttendance")]
public class RehearsalAttendancesController : Controller
{
    private ILogger<RehearsalAttendancesController> _logger;
    private readonly IRehearsalsService _rehearsalsService;
    private readonly IRehearsalAttendanceService _attendanceService;

    public RehearsalAttendancesController(
            ILogger<RehearsalAttendancesController> logger,
            IRehearsalsService rehearsalsService,
            IRehearsalAttendanceService attendanceService) {
        this._logger = logger;
        this._rehearsalsService = rehearsalsService;
        this._attendanceService = attendanceService;
    }
    
    [HttpGet]
    [Route("Index/{id}")]
    public async Task<ActionResult> Index(int id) {
        Rehearsal? rehearsal = await this._rehearsalsService.GetRehearsalByIdAsync(id);
        if (rehearsal is null) {
            return NotFound();
        }

        List<RehearsalAttendance> attendances = 
            await this._attendanceService.GetRehearsalAttendanceAsync(rehearsal);

        RehearsalAttendanceViewModel model = new RehearsalAttendanceViewModel {
            RehearsalId = id,
            RehearsalTitle = rehearsal.Title,
            Attendances = attendances.Select(a => new RehearsalAttendanceModel {
                Id = a.Id,
                Member = a.Member,
                IsPresent = a.IsPresent,
                ReasonForAbsence = a.ReasonForAbsence ?? ""
            }).ToList()
        };

        return View("RehearsalAttendance", model);
    }

    [HttpPost]
    [Route("Edit/{id}")]
    public async Task<ActionResult> Edit(int id, RehearsalAttendanceViewModel model) {
        Rehearsal? rehearsal = await this._rehearsalsService.GetRehearsalByIdAsync(id);
        if (rehearsal is null) {
            return NotFound();
        }

        List<RehearsalAttendance> attendances = 
            await this._attendanceService.GetRehearsalAttendanceAsync(rehearsal);

        List<RehearsalAttendance> editedAttendances = 
            await this._attendanceService.EditRehearsalAttendanceAsync(rehearsal, model.Attendances);

        return RedirectToAction("Index", new {rehearsalId = id});
    }
}
