namespace WebApp.RehearsalAttendances.Models;

public class RehearsalAttendanceViewModel {
    public int RehearsalId { get; set; }
    public string RehearsalTitle { get; set; } = "";
    public List<RehearsalAttendanceModel> Attendances { get; set; } = new List<RehearsalAttendanceModel>();
}
