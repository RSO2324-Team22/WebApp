namespace WebApp.Members;

public interface IMembersService
{
    Task<Member> CreateMemberAsync(NewMember newMember);
    Task DeleteMemberAsync(int id);
    Task<Member> EditMemberAsync(Member member);
    Task<Member> GetMemberByIdAsync(int id);
    Task<List<Member>> GetMembersAsync();
}
