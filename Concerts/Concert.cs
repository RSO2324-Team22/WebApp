using WebApp.Locations;

namespace WebApp.Concerts;

public class Concert
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public Location Location { get; set; } = new Location();
    public DateTime? MeetupTime { get; set; }
    public DateTime? SoundCheckTime { get; set; }
    public DateTime StartTime { get; set; } = default;
    public DateTime? ExpectedEndTime { get; set; }
    public string? Notes { get; set; }
    public ConcertStatus Status { get; set; }
}

public enum ConcertStatus {
    Proposed,
    InArrangement,
    AwaitingApproval,
    Confirmed,
    Cancelled
}
