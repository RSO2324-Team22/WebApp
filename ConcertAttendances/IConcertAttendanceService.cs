using WebApp.ConcertAttendances.Models;
using WebApp.Concerts;

namespace WebApp.ConcertAttendances;

public interface IConcertAttendanceService
{
    Task<List<ConcertAttendance>> AddConcertAttendanceAsync(Concert concert, List<ConcertAttendanceModel> attendances);
    Task<List<ConcertAttendance>> EditConcertAttendanceAsync(Concert concert, List<ConcertAttendanceModel> attendances);
    Task<List<ConcertAttendance>> GetConcertAttendanceAsync(Concert concert);
}
