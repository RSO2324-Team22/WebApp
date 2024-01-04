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
            id = this.Id,
            title = this.Title,
            location = new LocationS()
            {
                latitude = this.Location.Latitude,
                longitude = this.Location.Longitude
            },
            startTime = this.StartTime,
            endTime = this.EndTime,
            notes = this.Notes,
            status = this.Status,
            type = this.Type,
        };
        return concert;
    }

    public static EditRehearsalViewModel FromRehearsal(Rehearsal rehearsal) { 
        EditRehearsalViewModel model = new EditRehearsalViewModel {
            Id = rehearsal.id,
            Title = rehearsal.title,
            Location = new Location()
            {
                Latitude = rehearsal.location.latitude,
                Longitude = rehearsal.location.longitude,
            },
            StartTime = rehearsal.startTime,
            EndTime = rehearsal.endTime,
            Notes = rehearsal.notes,
            Status = rehearsal.status,
            Type = rehearsal.type,
        };
        return model;
    }
}