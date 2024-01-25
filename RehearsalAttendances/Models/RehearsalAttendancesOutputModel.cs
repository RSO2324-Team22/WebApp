namespace WebApp.RehearsalAttendances.Models;

public class RehearsalAttendanceOutputModel {
    public int MemberId { get; set; }
    public bool IsPresent { get; set; }
    public string ReasonForAbsence { get; set; } = "";
}
