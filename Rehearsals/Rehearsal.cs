using WebApp.Concerts.Models;
using WebApp.Rehearsals.Models;
using WebApp.Shared;

namespace WebApp.Rehearsals;

public class Rehearsal
{
    public int id { get; set; }
    public required string title { get; set; }
    public required LocationS location { get; set; }
    public required DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string? notes { get; set; }
    public RehearsalStatus status { get; set; }
    public RehearsalType type { get; set; }
}
