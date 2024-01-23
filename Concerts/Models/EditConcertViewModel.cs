using WebApp.Shared;

namespace WebApp.Concerts.Models;

public class EditConcertViewModel
{
    public int Id { get; set; } = 0;
    public string Title { get; set; } = "";
    public Location Location { get; set; } = new() {Latitude = 0, Longitude = 0};
    public DateTime? MeetupTime { get; set; } = DateTime.Now;
    public DateTime? SoundCheckTime { get; set; } = DateTime.Now;
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? ExpectedEndTime { get; set; } = DateTime.Now;
    public string? Notes { get; set; } = "";
    public ConcertStatus Status { get; set; } = ConcertStatus.Proposed;

    public Concert ToConcert() {
        Concert concert = new Concert {
            Id = this.Id,
            Title = this.Title,
            Location = this.Location,
            MeetupTime = this.MeetupTime,
            SoundCheckTime = this.SoundCheckTime,
            StartTime = this.StartTime,
            ExpectedEndTime = this.ExpectedEndTime,
            Notes = this.Notes,
            Status = this.Status
        };
        return concert;
    }

    public static EditConcertViewModel FromConcert(Concert concert) { 
        EditConcertViewModel model = new EditConcertViewModel {
            Id = concert.Id,
            Title = concert.Title,
            Location = concert.Location,
            MeetupTime = concert.MeetupTime,
            SoundCheckTime = concert.SoundCheckTime,
            StartTime = concert.StartTime,
            ExpectedEndTime = concert.ExpectedEndTime,
            Notes = concert.Notes,
            Status = concert.Status
        };
        return model;
    }
}
