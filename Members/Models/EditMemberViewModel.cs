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
            Id = this.Id,
            Name = this.Name,
            PhoneNumber = this.PhoneNumber,
            Email = this.Email,
            Section = this.Section,
            Roles = roles
        };
        return member;
    }

    public static EditMemberViewModel FromMember(Member member) { 
        EditMemberViewModel model = new EditMemberViewModel {
            Id = member.Id,
            Name = member.Name,
            PhoneNumber = member.PhoneNumber,
            Email = member.Email,
            Section = member.Section,
            SingerRole = member.Roles.Contains(Role.Singer),
            CouncilRole = member.Roles.Contains(Role.Council),
            ConductorRole = member.Roles.Contains(Role.Conductor)
        };
        return model;
    }
}
