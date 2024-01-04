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
            id = this.Id,
            title = this.Title,
            location = new LocationS()
            {
                latitude = this.Location.Latitude,
                longitude = this.Location.Longitude
            },
            meetupTime = this.MeetupTime,
            soundCheckTime = this.SoundCheckTime,
            startTime = this.StartTime,
            expectedEndTime = this.ExpectedEndTime,
            notes = this.Notes,
            status = this.Status
        };
        return concert;
    }

    public static EditConcertViewModel FromConcert(Concert concert) { 
        EditConcertViewModel model = new EditConcertViewModel {
            Id = concert.id,
            Title = concert.title,
            Location = new Location()
            {
                Latitude = concert.location.latitude,
                Longitude = concert.location.longitude,
            },
            MeetupTime = concert.meetupTime,
            SoundCheckTime = concert.soundCheckTime,
            StartTime = concert.startTime,
            ExpectedEndTime = concert.expectedEndTime,
            Notes = concert.notes,
            Status = concert.status
        };
        return model;
    }
}