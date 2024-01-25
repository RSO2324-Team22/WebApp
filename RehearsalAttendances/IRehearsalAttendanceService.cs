using WebApp.RehearsalAttendances.Models;
using WebApp.Rehearsals;

namespace WebApp.RehearsalAttendances;

public interface IRehearsalAttendanceService
{
    Task<List<RehearsalAttendance>> AddRehearsalAttendanceAsync(Rehearsal rehearsal, List<RehearsalAttendanceModel> attendances);
    Task<List<RehearsalAttendance>> EditRehearsalAttendanceAsync(Rehearsal rehearsal, List<RehearsalAttendanceModel> attendances);
    Task<List<RehearsalAttendance>> GetRehearsalAttendanceAsync(Rehearsal rehearsal);
}
