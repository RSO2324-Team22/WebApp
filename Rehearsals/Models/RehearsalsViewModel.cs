using WebApp.Locations;

namespace WebApp.Rehearsals.Models;

public class RehearsalsViewModel
{
    public IEnumerable<Rehearsal> Rehearsals { get; set; } = new List<Rehearsal>();

    public class Rehearsal {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required Location Location { get; set; }
        public required DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Notes { get; set; }
        public RehearsalStatus Status { get; set; }
        public RehearsalType Type { get; set; }
    }
}
