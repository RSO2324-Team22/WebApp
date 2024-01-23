using WebApp.Locations;

namespace WebApp.Rehearsals;

public class Rehearsal
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required Location Location { get; set; }
    public required DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Notes { get; set; }
    public RehearsalStatus Status { get; set; }
    public RehearsalType Type { get; set; }
}

public enum RehearsalStatus {
    Proposed,
    InArrangement,
    AwaitingApproval,
    Confirmed,
    Cancelled
}

public enum RehearsalType
{
    Regular,
    Extra,
    Intensive   
}
