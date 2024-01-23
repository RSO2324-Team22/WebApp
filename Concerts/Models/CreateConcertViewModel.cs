using WebApp.Shared;

namespace WebApp.Concerts.Models;

public class CreateConcertViewModel
{
    public string Title { get; set; } = "";
    public Location Location { get; set; } = new() {Latitude = 0, Longitude = 0};
    public DateTime? MeetupTime { get; set; } = DateTime.Now;
    public DateTime? SoundCheckTime { get; set; } = DateTime.Now;
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? ExpectedEndTime { get; set; } =DateTime.Now;
    public string? Notes { get; set; } = "";
    public string Status { get; set; } = "";

    public CreateConcertModel ToNewConcert() {
        CreateConcertModel newConcert = new CreateConcertModel {
            Title = this.Title,
            Location = this.Location,
            MeetupTime = this.MeetupTime,
            SoundCheckTime = this.SoundCheckTime,
            StartTime = this.StartTime,
            ExpectedEndTime = this.ExpectedEndTime,
            Notes = this.Notes,
            Status = Enum.Parse<ConcertStatus>(this.Status)
        };
        return newConcert;
    }

    public static CreateConcertViewModel FromNewConcert(CreateConcertModel concert) { 
        CreateConcertViewModel model = new CreateConcertViewModel {
            Title = concert.Title,
            Location = concert.Location,
            MeetupTime = concert.MeetupTime,
            SoundCheckTime = concert.SoundCheckTime,
            StartTime = concert.StartTime,
            ExpectedEndTime = concert.ExpectedEndTime,
            Notes = concert.Notes,
            Status = concert.Status.ToString()
        };
        return model;
    }
}
