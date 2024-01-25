namespace WebApp.ConcertAttendances.Models;

public class ConcertAttendanceOutputModel {
    public int MemberId { get; set; }
    public bool IsPresent { get; set; }
    public string ReasonForAbsence { get; set; } = "";
}
