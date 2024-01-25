using WebApp.Members;

namespace WebApp.ConcertAttendances.Models;

public class ConcertAttendanceModel {
    public int Id { get; set; }
    public required Member Member { get; set; }
    public bool IsPresent { get; set; }
    public string ReasonForAbsence { get; set; } = "";
}
