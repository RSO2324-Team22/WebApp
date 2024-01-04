using WebApp.Concerts.Models;
using WebApp.Shared;

namespace WebApp.Concerts;

public class Concert
{
    public int id { get; set; }
    public required string title { get; set; }
    public required LocationS location { get; set; }
    public DateTime? meetupTime { get; set; }
    public DateTime? soundCheckTime { get; set; }
    public required DateTime startTime { get; set; }
    public DateTime? expectedEndTime { get; set; }
    public string? notes { get; set; }
    public ConcertStatus status { get; set; }
}
