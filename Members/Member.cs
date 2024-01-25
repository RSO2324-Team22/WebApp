namespace WebApp.Members;

public class Member {
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Email { get; set; } = "";
    public Section Section { get; set; }
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();
}

public class CreateMemberModel {
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required Section Section { get; set; }
    public required IEnumerable<Role> Roles { get; set; }
}

public enum Section {
    TenorOne, TenorTwo, Baritone, Bass
}

public enum Role {
    Singer, Conductor, Council
}

