using System.Text.Json;
using WebApp.Rehearsals.Models;
using WebApp.Shared;

namespace WebApp.Rehearsals;

public class RehearsalService : IRehearsalService
{
    private readonly ILogger<RehearsalService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string? _serviceUrl;

    public RehearsalService(
        ILogger<RehearsalService> logger,
        IConfiguration config)
    {
        this._logger = logger;
        this._httpClient = new HttpClient();
        this._serviceUrl = config["REHEARSALS_SERVICE_URL"];
    }

    public async Task<List<Rehearsal>> GetRehearsalsAsync()
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
            return JsonSerializer.Deserialize<List<Rehearsal>>(body);
        }
        catch (JsonException e)
        {
            this._logger.LogError("An error occurred while fetching Rehearsals \n" + e.Message);
            throw;
        }
    }

    public async Task<Rehearsal> GetRehearsalByIdAsync(int id)
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
            this._logger.LogError($"An error occured while fetching Rehearsal Id {id} \n" + e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Rehearsal>(body);
        }
        catch (Exception e)
        {
            this._logger.LogError($"An error occured while deserializing Rehearsal Id {id} \n" + e.Message);
            throw;
        }
    }

    public async Task<Rehearsal> EditRehearsalAsync(Rehearsal rehearsal)
    {
        string body;
        try
        {
            var editedConcert = new NewRehearsal
            {
                Title = rehearsal.title,
                Location = new Location()
                {
                    Latitude = rehearsal.location.latitude,
                    Longitude = rehearsal.location.longitude,
                },
                StartTime = rehearsal.startTime,
                EndTime = rehearsal.endTime,
                Notes = rehearsal.notes,
                Status = rehearsal.status
            };

            JsonContent json = JsonContent.Create<NewRehearsal>(editedConcert);
            var neki = await json.ReadAsStringAsync();
            using HttpResponseMessage response =
                await this._httpClient.PutAsync(this._serviceUrl + "/" + rehearsal.id, json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while fetching Rehearsal Id {rehearsal.id} \n" + e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Rehearsal>(body);
        }
        catch (Exception e)
        {
            this._logger.LogError($"An error occured while deserializing Rehearsal Id {rehearsal.id} \n" + e.Message);
            throw;
        }
    }

    public async Task<Rehearsal> CreateRehearsalAsync(NewRehearsal newRehearsal)
    {
        string body;
        try
        {
            JsonContent json = JsonContent.Create<NewRehearsal>(newRehearsal);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync(this._serviceUrl, json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while creating a new Rehearsal \n" + e.Message);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Rehearsal>(body);
        }
        catch (Exception e)
        {
            this._logger.LogError($"An error occured while deserializing a new Rehearsal \n" + e.Message);
            throw;
        }
    }

    public async Task DeleteRehearsalAsync(int id)
    {
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync(this._serviceUrl + "/" + id);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError($"An error occured while fetching Rehearsal Id {id} \n" + e.Message);
            throw;
        }
    }
}