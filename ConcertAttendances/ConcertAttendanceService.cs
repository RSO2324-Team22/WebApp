using System.Text.Json;
using WebApp.ConcertAttendances.Models;
using WebApp.Concerts;

namespace WebApp.ConcertAttendances;

public class ConcertAttendanceService : IConcertAttendanceService
{
    private readonly ILogger<ConcertAttendanceService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializeOptions;

    public ConcertAttendanceService(
        ILogger<ConcertAttendanceService> logger,
        HttpClient httpClient)
    {
        this._logger = logger;
        this._httpClient = httpClient;
        this._serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<ConcertAttendance>> GetConcertAttendanceAsync(Concert concert)
    {
        this._logger.LogInformation("Fetching attendance for concert {id}", concert.Id);
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync($"concerts/concert/{concert.Id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ConcertAttendance>>(body, this._serializeOptions)!;
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

    public async Task<List<ConcertAttendance>> AddConcertAttendanceAsync(
            Concert concert,
            List<ConcertAttendanceModel> attendances)
    {
        this._logger.LogInformation("Adding attendance for concert {id}", concert.Id);
        string body;
        try
        {
            JsonContent json = JsonContent.Create<List<ConcertAttendanceModel>>(attendances);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync($"concerts/concert/{concert.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            var edited = JsonSerializer.Deserialize<List<ConcertAttendance>>(body, this._serializeOptions)!;
            this._logger.LogInformation("Added attendance for concert {id}", concert.Id);
            return edited;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while adding attendance for concert {id}", concert.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<List<ConcertAttendance>> EditConcertAttendanceAsync(
            Concert concert,
            List<ConcertAttendanceModel> attendances)
    {
        List<ConcertAttendanceOutputModel> outputModels = attendances
            .Select(a => new ConcertAttendanceOutputModel {
                MemberId = a.Member.Id,
                IsPresent = a.IsPresent,
                ReasonForAbsence = a.ReasonForAbsence
            })
            .ToList();

        this._logger.LogInformation("Edited attendance for concert {id}", concert.Id);
        string body;
        try
        {
            JsonContent json = JsonContent.Create<List<ConcertAttendanceOutputModel>>(outputModels);
            using HttpResponseMessage response =
                await this._httpClient.PatchAsync($"concerts/concert/{concert.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            var edited = JsonSerializer.Deserialize<List<ConcertAttendance>>(body, this._serializeOptions)!;
            this._logger.LogInformation("Edited attendance for concert {id}", concert.Id);
            return edited;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while editing attendance for concert {id}", concert.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }
}
