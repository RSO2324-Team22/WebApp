namespace WebApp.Members;

public class Member {
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required Section Section { get; set; }
    public required IEnumerable<Role> Roles { get; set; }
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

