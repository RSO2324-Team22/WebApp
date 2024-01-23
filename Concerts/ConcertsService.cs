using System.Net;
using System.Text.Json;
using WebApp.Concerts.Models;
using WebApp.Locations;

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
        this._logger.LogInformation("Fetching concerts");
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Concert>>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error has occurred while fetching concerts");
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert?> GetConcertByIdAsync(int id)
    {
        this._logger.LogInformation("Fetching concert {id}", id);
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync($"concert/{id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Concert>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            this._logger.LogError(e, "An error occured while fetching concert {id}", id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert?> EditConcertAsync(Concert concert)
    {
        this._logger.LogInformation("Editing concert {id}", concert.Id);
        string body;
        try
        {
            var editedConcert = new CreateConcertModel
            {
                Title = concert.Title,
                Location = concert.Location,
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
                await this._httpClient.PutAsync($"concert/{concert.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Concert>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            this._logger.LogError(e, "An error occured while editing concert {id}", concert.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert> CreateConcertAsync(CreateConcertModel newConcert)
    {
        this._logger.LogInformation("Adding new concert");
        string body;
        try
        {
            JsonContent json = JsonContent.Create<CreateConcertModel>(newConcert);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync("", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Concert concert = JsonSerializer.Deserialize<Concert>(body, this._serializeOptions)!;
            this._logger.LogInformation("Added concert {id}", concert.Id);
            return concert;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while creating a new concert");
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Concert?> DeleteConcertAsync(int id)
    {
        this._logger.LogInformation("Deleting concert {id}", id);
        string body;
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync($"concert/{id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Concert concert = JsonSerializer.Deserialize<Concert>(body, this._serializeOptions)!;
            return concert;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            this._logger.LogError(e, "An error occured while fetching concert {id}", id);
            throw;
        }
    }
}
