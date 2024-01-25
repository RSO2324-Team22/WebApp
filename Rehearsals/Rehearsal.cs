using WebApp.Locations;

namespace WebApp.Rehearsals;

public class Rehearsal
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public Location Location { get; set; } = new Location();
    public DateTime StartTime { get; set; } = default;
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
