namespace WebApp.Members;

public class EditMemberViewModel {
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Section { get; set; }
    public required bool SingerRole { get; set; }
    public required bool CouncilRole { get; set; }
    public required bool ConductorRole { get; set; }
}
