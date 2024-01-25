using System.Text.Json;
using WebApp.RehearsalAttendances.Models;
using WebApp.Rehearsals;

namespace WebApp.RehearsalAttendances;

public class RehearsalAttendanceService : IRehearsalAttendanceService
{
    private readonly ILogger<RehearsalAttendanceService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializeOptions;

    public RehearsalAttendanceService(
        ILogger<RehearsalAttendanceService> logger,
        HttpClient httpClient)
    {
        this._logger = logger;
        this._httpClient = httpClient;
        this._serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<RehearsalAttendance>> GetRehearsalAttendanceAsync(Rehearsal rehearsal)
    {
        this._logger.LogInformation("Fetching attendance for rehearsal {id}", rehearsal.Id);
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync($"rehearsals/rehearsal/{rehearsal.Id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RehearsalAttendance>>(body, this._serializeOptions)!;
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

    public async Task<List<RehearsalAttendance>> AddRehearsalAttendanceAsync(
            Rehearsal rehearsal,
            List<RehearsalAttendanceModel> attendances)
    {
        this._logger.LogInformation("Adding attendance for rehearsal {id}", rehearsal.Id);
        string body;
        try
        {
            JsonContent json = JsonContent.Create<List<RehearsalAttendanceModel>>(attendances);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync($"rehearsals/rehearsal/{rehearsal.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            var edited = JsonSerializer.Deserialize<List<RehearsalAttendance>>(body, this._serializeOptions)!;
            this._logger.LogInformation("Added attendance for rehearsal {id}", rehearsal.Id);
            return edited;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while adding attendance for rehearsal {id}", rehearsal.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<List<RehearsalAttendance>> EditRehearsalAttendanceAsync(
            Rehearsal rehearsal,
            List<RehearsalAttendanceModel> attendances)
    {
        List<RehearsalAttendanceOutputModel> outputModels = attendances
            .Select(a => new RehearsalAttendanceOutputModel {
                MemberId = a.Member.Id,
                IsPresent = a.IsPresent,
                ReasonForAbsence = a.ReasonForAbsence
            })
            .ToList();

        this._logger.LogInformation("Edited attendance for rehearsal {id}", rehearsal.Id);
        string body;
        try
        {
            JsonContent json = JsonContent.Create<List<RehearsalAttendanceOutputModel>>(outputModels);
            using HttpResponseMessage response =
                await this._httpClient.PatchAsync($"rehearsals/rehearsal/{rehearsal.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            var edited = JsonSerializer.Deserialize<List<RehearsalAttendance>>(body, this._serializeOptions)!;
            this._logger.LogInformation("Edited attendance for rehearsal {id}", rehearsal.Id);
            return edited;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while editing attendance for rehearsal {id}", rehearsal.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }
}
