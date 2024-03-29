namespace WebApp.Members;

public interface IMembersService
{
    Task<Member> CreateMemberAsync(CreateMemberModel newMember);
    Task<Member?> DeleteMemberAsync(int id);
    Task<Member?> EditMemberAsync(Member member);
    Task<Member?> GetMemberByIdAsync(int id);
    Task<List<Member>> GetMembersAsync();
}
