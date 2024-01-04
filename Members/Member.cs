namespace WebApp.Members;

public class Member {
    public required int id { get; set; }
    public required string name { get; set; }
    public required string phoneNumber { get; set; }
    public required string email { get; set; }
    public required Section section { get; set; }
    public required IEnumerable<Role> roles { get; set; }
}

public class NewMember {
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

