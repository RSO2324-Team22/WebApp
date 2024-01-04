
using System.Text.Json;

namespace WebApp.Members;

public class MembersService
{
    private readonly ILogger<MembersService> _logger;
    private readonly HttpClient _httpClient;
    private readonly string? _serviceUrl;

    public MembersService(
            ILogger<MembersService> logger,
            IConfiguration config) {
        this._logger = logger;
        this._httpClient = new HttpClient();
        this._serviceUrl = config["MEMBERS_SERVICE_URL"];
    }

    public async Task<List<Member>> GetMembersAsync() {
        string body;
        try {
            using HttpResponseMessage response = await this._httpClient.GetAsync(this._serviceUrl);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e) {
            this._logger.LogError(e.Message);
            throw;
        }

        try {
            return JsonSerializer.Deserialize<List<Member>>(body);
        }
        catch (JsonException e) {
            this._logger.LogError("An error occurred while fetching members \n" + e.Message);
            throw;
        }
    }

    public async Task<Member> GetMemberByIdAsync(int id) {
        string body;
        try {
            using HttpResponseMessage response = await this._httpClient.GetAsync(this._serviceUrl + "/" + id);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e) {
            this._logger.LogError($"An error occured while fetching member Id {id} \n" + e.Message);
            throw;
        }

        try {
            return JsonSerializer.Deserialize<Member>(body);
        }
        catch (Exception e) {
            this._logger.LogError($"An error occured while deserializing member Id {id} \n" + e.Message);
            throw;
        }
    }

    public async Task<Member> EditMemberAsync(Member member) {
        string body;
        try {
            JsonContent json = JsonContent.Create<Member>(member);
            using HttpResponseMessage response = 
                await this._httpClient.PutAsync(this._serviceUrl + "/" + member.Id, json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e) {
            this._logger.LogError($"An error occured while fetching member Id {member.Id} \n" + e.Message);
            throw;
        }

        try {
            return JsonSerializer.Deserialize<Member>(body);
        }
        catch (Exception e) {
            this._logger.LogError($"An error occured while deserializing member Id {member.Id} \n" + e.Message);
            throw;
        }
    }

    public async Task<Member> CreateMemberAsync(NewMember newMember) {
        string body;
        try {
            JsonContent json = JsonContent.Create<NewMember>(newMember);
            using HttpResponseMessage response = 
                await this._httpClient.PostAsync(this._serviceUrl, json);
            response.EnsureSuccessStatusCode();
            body = await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e) {
            this._logger.LogError($"An error occured while creating a new member \n" + e.Message);
            throw;
        }

        try {
            return JsonSerializer.Deserialize<Member>(body);
        }
        catch (Exception e) {
            this._logger.LogError($"An error occured while deserializing a new member \n" + e.Message);
            throw;
        }
    }

    public async Task DeleteMemberAsync(int id)
    {
        try {
            using HttpResponseMessage response = 
                await this._httpClient.DeleteAsync(this._serviceUrl + "/" + id);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e) {
            this._logger.LogError($"An error occured while fetching member Id {id} \n" + e.Message);
            throw;
        }
    }
}
