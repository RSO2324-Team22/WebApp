using System.Text.Json;
using WebApp.Rehearsals.Models;

namespace WebApp.Rehearsals;

public class RehearsalsService : IRehearsalsService
{
    private readonly ILogger<RehearsalsService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializeOptions;

    public RehearsalsService(
        ILogger<RehearsalsService> logger,
        HttpClient httpClient)
    {
        this._logger = logger;
        this._httpClient = httpClient;
        this._serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<Rehearsal>> GetRehearsalsAsync()
    {
        this._logger.LogInformation("Fetching rehearsals");
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Rehearsal>>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error has occurred while fetching rehearsals");
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Rehearsal> GetRehearsalByIdAsync(int id)
    {
        this._logger.LogInformation("Fetching rehearsal {id}", id);
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync($"{id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Rehearsal>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while fetching rehearsal {id}", id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Rehearsal> EditRehearsalAsync(Rehearsal rehearsal)
    {
        this._logger.LogInformation("Editing rehearsal {id}", rehearsal.Id);
        string body;
        try
        {
            var editedRehearsal = new CreateRehearsalModel
            {
                Title = rehearsal.Title,
                Location = rehearsal.Location,
                StartTime = rehearsal.StartTime,
                EndTime = rehearsal.EndTime,
                Notes = rehearsal.Notes,
                Status = rehearsal.Status,
                Type = rehearsal.Type
            };

            JsonContent json = JsonContent.Create<CreateRehearsalModel>(editedRehearsal);
            var neki = await json.ReadAsStringAsync();
            using HttpResponseMessage response =
                await this._httpClient.PutAsync($"{rehearsal.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Rehearsal>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while editing rehearsal {id}", rehearsal.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Rehearsal> CreateRehearsalAsync(CreateRehearsalModel newRehearsal)
    {
        this._logger.LogInformation("Adding new rehearsal");
        string body;
        try
        {
            JsonContent json = JsonContent.Create<CreateRehearsalModel>(newRehearsal);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync("", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Rehearsal rehearsal = JsonSerializer.Deserialize<Rehearsal>(body, this._serializeOptions)!;
            this._logger.LogInformation("Added rehearsal {id}", rehearsal.Id);
            return rehearsal;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while creating a new rehearsal");
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task DeleteRehearsalAsync(int id)
    {
        this._logger.LogInformation("Deleting rehearsal {id}", id);
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while fetching rehearsal {id}", id);
            throw;
        }
    }
}
