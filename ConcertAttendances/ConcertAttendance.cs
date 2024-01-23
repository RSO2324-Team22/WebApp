using WebApp.Concerts;
using WebApp.Members;

namespace WebApp.ConcertAttendances;

public class ConcertAttendance {
    public int Id { get; set; }
    public required Member Member { get; set; }
    public required Concert Concert { get; set; }
    public required bool IsPresent { get; set; }
    public string? ReasonForAbsence { get; set; }
}
