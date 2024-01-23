using System.Text.Json;
using WebApp.Rehearsals.Models;
using WebApp.Shared;

namespace WebApp.Rehearsals;

public class RehearsalService : IRehearsalService
{
    private readonly ILogger<RehearsalService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializeOptions;

    public RehearsalService(
        ILogger<RehearsalService> logger,
        HttpClient httpClient)
    {
        this._logger = logger;
        this._httpClient = httpClient;
        this._serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<Rehearsal>> GetRehearsalsAsync()
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
            this._logger.LogError(e, "An error occurred while fetching rehearsals.");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<List<Rehearsal>>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Rehearsal> GetRehearsalByIdAsync(int id)
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
            this._logger.LogError(e, "An error occured while fetching rehearsal {id}", id);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Rehearsal>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Rehearsal> EditRehearsalAsync(Rehearsal rehearsal)
    {
        string body;
        try
        {
            var editedRehearsal = new CreateRehearsalModel
            {
                Title = rehearsal.Title,
                Location = new Location()
                {
                    Latitude = rehearsal.Location.Latitude,
                    Longitude = rehearsal.Location.Longitude,
                },
                StartTime = rehearsal.StartTime,
                EndTime = rehearsal.EndTime,
                Notes = rehearsal.Notes,
                Status = rehearsal.Status
            };

            JsonContent json = JsonContent.Create<CreateRehearsalModel>(editedRehearsal);
            var neki = await json.ReadAsStringAsync();
            using HttpResponseMessage response =
                await this._httpClient.PutAsync($"{rehearsal.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while editing rehearsal {id}", rehearsal.Id);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Rehearsal>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Rehearsal> CreateRehearsalAsync(CreateRehearsalModel newRehearsal)
    {
        string body;
        try
        {
            JsonContent json = JsonContent.Create<CreateRehearsalModel>(newRehearsal);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync("", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while creating a new rehearsal");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Rehearsal>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task DeleteRehearsalAsync(int id)
    {
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while deleting rehearsal {id}", id);
            throw;
        }
    }
}
