using System.Text.Json;
using WebApp.Concerts.Models;
using WebApp.Shared;

namespace WebApp.Concerts;

public class ConcertService : IConcertService
{
    private readonly ILogger<ConcertService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string? _serviceUrl;

    public ConcertService(
        ILogger<ConcertService> logger,
        IConfiguration config)
    {
        this._logger = logger;
        this._httpClient = new HttpClient();
        this._serviceUrl = config["CONCERTS_SERVICE_URL"];
    }

    public async Task<List<Concert>> GetConcertsAsync()
    {
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync(this._serviceUrl);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<List<Concert>>(body);
        }
        catch (JsonException e)
        {
            this._logger.LogError("An error occurred while fetching concerts \n" + e.Message);
            throw;
        }
    }

    public async Task<Concert> GetConcertByIdAsync(int id)
    {
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync(this._serviceUrl + "/" + id);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while fetching concert Id {id} \n" + e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Concert>(body);
        }
        catch (Exception e)
        {
            this._logger.LogError($"An error occured while deserializing concert Id {id} \n" + e.Message);
            throw;
        }
    }

    public async Task<Concert> EditConcertAsync(Concert concert)
    {
        string body;
        try
        {
            var editedConcert = new NewConcert
            {
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

            JsonContent json = JsonContent.Create<NewConcert>(editedConcert);
            var neki = await json.ReadAsStringAsync();
            using HttpResponseMessage response =
                await this._httpClient.PutAsync(this._serviceUrl + "/" + concert.id, json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while fetching concert Id {concert.id} \n" + e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Concert>(body);
        }
        catch (Exception e)
        {
            this._logger.LogError($"An error occured while deserializing concert Id {concert.id} \n" + e.Message);
            throw;
        }
    }

    public async Task<Concert> CreateConcertAsync(NewConcert newMember)
    {
        string body;
        try
        {
            JsonContent json = JsonContent.Create<NewConcert>(newMember);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync(this._serviceUrl, json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while creating a new concert \n" + e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Concert>(body);
        }
        catch (Exception e)
        {
            this._logger.LogError($"An error occured while deserializing a new concert \n" + e.Message);
            throw;
        }
    }

    public async Task DeleteConcertAsync(int id)
    {
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync(this._serviceUrl + "/" + id);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while fetching concert Id {id} \n" + e.Message);
            throw;
        }
    }
}