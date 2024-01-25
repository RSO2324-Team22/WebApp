namespace WebApp.ConcertAttendances.Models;

public class ConcertAttendanceViewModel {
    public int ConcertId { get; set; }
    public string ConcertTitle { get; set; } = "";
    public List<ConcertAttendanceModel> Attendances { get; set; } = new List<ConcertAttendanceModel>();
}
