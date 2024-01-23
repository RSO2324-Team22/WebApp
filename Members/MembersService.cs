using System.Text.Json;
using WebApp.Members.Models;

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
        string body;
        try
        {
            using HttpResponseMessage response = await this._httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occurred while fetching members");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<List<Member>>(body, this._serializeOptions) ?? new List<Member>();
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member> GetMemberByIdAsync(int id)
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
            this._logger.LogError(e, "An error occured while fetching member {id}", id);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Member>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member> EditMemberAsync(Member member)
    {
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
                await this._httpClient.PutAsync($"{member.Id}", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while fetching member {id}", member.Id);
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Member>(body, this._serializeOptions);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task<Member> CreateMemberAsync(CreateMemberModel newMember)
    {
        string body;
        try
        {
            JsonContent json = JsonContent.Create<CreateMemberModel>(newMember, null, this._serializeOptions);
            using HttpResponseMessage response =
                await this._httpClient.PostAsync("", json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while creating a new member");
            throw;
        }

        try
        {
            return JsonSerializer.Deserialize<Member>(body);
        }
        catch (JsonException e)
        {
            this._logger.LogError(e, "Response body could not be deserialized");
            throw;
        }
    }

    public async Task DeleteMemberAsync(int id)
    {
        try
        {
            using HttpResponseMessage response =
                await this._httpClient.DeleteAsync($"{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            this._logger.LogError(e, "An error occured while fetching member {id}");
            throw;
        }
    }
}
