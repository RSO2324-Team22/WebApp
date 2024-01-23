using WebApp.Locations;

namespace WebApp.Rehearsals.Models;

public class CreateRehearsalViewModel
{
    public string Title { get; set; } = "";
    public Location Location { get; set; } = new() {Latitude = 0, Longitude = 0};
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
    public string? Notes { get; set; } = "";
    public string Status { get; set; } = "";
    public string Type { get; set; } = "";

    public CreateRehearsalModel ToNewRehearsal() {
        CreateRehearsalModel newRehearsal = new CreateRehearsalModel {
            Title = this.Title,
            Location = this.Location,
            StartTime = this.StartTime,
            EndTime = this.EndTime,
            Notes = this.Notes,
            Status = Enum.Parse<RehearsalStatus>(this.Status),
            Type = Enum.Parse<RehearsalType>(this.Type)
        };
        return newRehearsal;
    }

    public static CreateRehearsalViewModel FromNewConcert(CreateRehearsalModel rehearsal) { 
        CreateRehearsalViewModel model = new CreateRehearsalViewModel {
            Title = rehearsal.Title,
            Location = rehearsal.Location,
            StartTime = rehearsal.StartTime,
            EndTime = rehearsal.EndTime,
            Notes = rehearsal.Notes,
            Status = rehearsal.Status.ToString(),
            Type = rehearsal.Type.ToString()
        };
        return model;
    }
}
