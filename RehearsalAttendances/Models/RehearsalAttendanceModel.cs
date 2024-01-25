using WebApp.Members;

namespace WebApp.RehearsalAttendances.Models;

public class RehearsalAttendanceModel {
    public int Id { get; set; }
    public required Member Member { get; set; }
    public bool IsPresent { get; set; }
    public string ReasonForAbsence { get; set; } = "";
}
