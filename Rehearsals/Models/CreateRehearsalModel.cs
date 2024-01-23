using WebApp.Rehearsals.Models;
using WebApp.Shared;

namespace WebApp.Rehearsals.Models;

public class CreateRehearsalModel
{
        public required string Title { get; set; }
        public required Location Location { get; set; }
        public required DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Notes { get; set; }
        public RehearsalStatus Status { get; set; }
        public RehearsalType Type { get; set; }
}
