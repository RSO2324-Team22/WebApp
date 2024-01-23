using WebApp.Shared;

namespace WebApp.Concerts;

public class Concert
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required Location Location { get; set; }
    public DateTime? MeetupTime { get; set; }
    public DateTime? SoundCheckTime { get; set; }
    public required DateTime StartTime { get; set; }
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
