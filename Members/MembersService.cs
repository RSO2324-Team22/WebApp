using System.Net;
using System.Text.Json;

namespace WebApp.Members;

public class MembersService : IMembersService
{
    private readonly ILogger<MembersService> _logger;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializeOptions;

    public MembersService(
            ILogger<MembersService> logger,
            HttpClient httpClient)
    {
        this._logger = logger;
        this._httpClient = httpClient;
        this._serializeOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }

    public async Task<List<Member>> GetMembersAsync()
    {
        this._logger.LogInformation("Fetching members");
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Member>>(body, this._serializeOptions) ?? new List<Member>();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occurred while fetching members");
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member?> GetMemberByIdAsync(int id)
    {
        this._logger.LogInformation("Fetching member {id}", id);
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync($"member/{id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Member>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode is not null && e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            this._logger.LogError(e, "An error occured while fetching member {id}", id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member?> EditMemberAsync(Member member)
    {
        this._logger.LogInformation("Editing member {id}", member.Id);
        string body;
        try
        {
            var editedMember = new CreateMemberModel
            {
                Name = member.Name,
                PhoneNumber = member.PhoneNumber,
                Email = member.Email,
                Section = member.Section,
                Roles = member.Roles
            };

            JsonContent json = JsonContent.Create<CreateMemberModel>(editedMember, null, this._serializeOptions);
            using HttpResponseMessage response =
                await this._httpClient.PutAsync($"member/{member.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Member>(body, this._serializeOptions)!;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode is not null && e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            this._logger.LogError(e, "An error occured while fetching member {id}", member.Id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member> CreateMemberAsync(CreateMemberModel newMember)
    {
        this._logger.LogInformation("Adding new member");
        string body;
        try
        {
            JsonContent json = JsonContent.Create<CreateMemberModel>(newMember, null, this._serializeOptions);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync("", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            Member member = JsonSerializer.Deserialize<Member>(body)!;
            return member;
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while adding a new member");
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member?> DeleteMemberAsync(int id)
    {
        this._logger.LogInformation("Deleting member {id}", id);
        string body;
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync($"member/{id}");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Member>(body)!;
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode is not null && e.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }

            this._logger.LogError(e, "An error occured while fetching member {id}", id);
            throw;
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }
}
