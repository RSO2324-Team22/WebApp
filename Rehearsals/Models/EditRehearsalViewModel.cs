using WebApp.Shared;

namespace WebApp.Rehearsals.Models;

public class EditRehearsalViewModel
{
    public int Id { get; set; } = 0;
    public string Title { get; set; } = "";
    public Location Location { get; set; } = new() {Latitude = 0, Longitude = 0};
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; } = DateTime.Now;
    public string? Notes { get; set; } = "";
    public RehearsalStatus Status { get; set; } = RehearsalStatus.Proposed;
    public RehearsalType Type { get; set; } = RehearsalType.Regular;

    public Rehearsal ToRehearsal() {
        Rehearsal concert = new Rehearsal {
            Id = this.Id,
            Title = this.Title,
            Location = this.Location,
            StartTime = this.StartTime,
            EndTime = this.EndTime,
            Notes = this.Notes,
            Status = this.Status,
            Type = this.Type,
        };
        return concert;
    }

    public static EditRehearsalViewModel FromRehearsal(Rehearsal rehearsal) { 
        EditRehearsalViewModel model = new EditRehearsalViewModel {
            Id = rehearsal.Id,
            Title = rehearsal.Title,
            Location = rehearsal.Location,
            StartTime = rehearsal.StartTime,
            EndTime = rehearsal.EndTime,
            Notes = rehearsal.Notes,
            Status = rehearsal.Status,
            Type = rehearsal.Type,
        };
        return model;
    }
}
