namespace WebApp.Members.Models;

public class EditMemberViewModel {
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public Section Section { get; set; } = Section.TenorOne;
    public bool SingerRole { get; set; } = true;
    public bool CouncilRole { get; set; } = false;
    public bool ConductorRole { get; set; } = false;

    public Member ToMember() {
        List<Role> roles = new List<Role>();
        if (this.SingerRole) roles.Add(Role.Singer);
        if (this.CouncilRole) roles.Add(Role.Council);
        if (this.ConductorRole) roles.Add(Role.Conductor);
        
        Member member = new Member {
            id = this.Id,
            name = this.Name,
            phoneNumber = this.PhoneNumber,
            email = this.Email,
            section = this.Section,
            roles = roles
        };
        return member;
    }

    public static EditMemberViewModel FromMember(Member member) { 
        EditMemberViewModel model = new EditMemberViewModel {
            Id = member.id,
            Name = member.name,
            PhoneNumber = member.phoneNumber,
            Email = member.email,
            Section = member.section,
            SingerRole = member.roles.Contains(Role.Singer),
            CouncilRole = member.roles.Contains(Role.Council),
            ConductorRole = member.roles.Contains(Role.Conductor)
        };
        return model;
    }
}
