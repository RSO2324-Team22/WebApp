using System.Text.Json;
using WebApp.Concerts.Models;
using WebApp.Shared;

namespace WebApp.Concerts;

public class ConcertsService : IConcertsService
{
    private readonly ILogger<ConcertsService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializeOptions;

    public ConcertsService(
        ILogger<ConcertsService> logger,
        HttpClient httpClient)
    {
        this._logger = logger;
        this._httpClient = httpClient;
        this._serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<Concert>> GetConcertsAsync()
    {
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error has occurred while fetching concerts");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<List<Concert>>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert> GetConcertByIdAsync(int id)
    {
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync($"{id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while fetching concert {id}");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Concert>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert> EditConcertAsync(Concert concert)
    {
        string body;
        try
        {
            var editedConcert = new CreateConcertModel
            {
                Title = concert.Title,
                Location = new Location()
                {
                    Latitude = concert.Location.Latitude,
                    Longitude = concert.Location.Longitude,
                },
                MeetupTime = concert.MeetupTime,
                SoundCheckTime = concert.SoundCheckTime,
                StartTime = concert.StartTime,
                ExpectedEndTime = concert.ExpectedEndTime,
                Notes = concert.Notes,
                Status = concert.Status
            };

            JsonContent json = JsonContent.Create<CreateConcertModel>(editedConcert);
            var neki = await json.ReadAsStringAsync();
            using HttpResponseMessage response =
                await this._httpClient.PutAsync($"{concert.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while editing concert {id}", concert.Id);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Concert>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert> CreateConcertAsync(CreateConcertModel newConcert)
    {
        string body;
        try
        {
            JsonContent json = JsonContent.Create<CreateConcertModel>(newConcert);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync("", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while creating a new concert");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Concert>(body, this._serializeOptions);
        }
        catch (Exception e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task DeleteConcertAsync(int id)
    {
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while fetching concert {id}", id);
            throw;
        }
    }
}
