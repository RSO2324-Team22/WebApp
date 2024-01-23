namespace WebApp.Members.Models;

public class CreateMemberViewModel {
    public string Name { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public string Section { get; set; } = "";
    public bool SingerRole { get; set; } = true;
    public bool CouncilRole { get; set; } = false;
    public bool ConductorRole { get; set; } = false;

    public CreateMemberModel ToModel() {
        List<Role> roles = new List<Role>();
        if (this.SingerRole) roles.Add(Role.Singer);
        if (this.CouncilRole) roles.Add(Role.Council);
        if (this.ConductorRole) roles.Add(Role.Conductor);
        
        CreateMemberModel newMember = new CreateMemberModel {
            Name = this.Name,
            PhoneNumber = this.PhoneNumber,
            Email = this.Email,
            Section = Enum.Parse<Section>(this.Section),
            Roles = roles
        };
        return newMember;
    }

    public static CreateMemberViewModel FromModel(CreateMemberModel member) { 
        CreateMemberViewModel model = new CreateMemberViewModel {
            Name = member.Name,
            PhoneNumber = member.PhoneNumber,
            Email = member.Email,
            Section = member.Section.ToString(),
            SingerRole = member.Roles.Contains(Role.Singer),
            CouncilRole = member.Roles.Contains(Role.Council),
            ConductorRole = member.Roles.Contains(Role.Conductor)
        };
        return model;
    }
}
