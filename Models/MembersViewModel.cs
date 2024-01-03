namespace WebApp.Models;

public class MembersViewModel {
    public IEnumerable<Member> Members { get; set; } = new List<Member>();

    public class Member {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Section { get; set; }
        public required IEnumerable<string> Roles { get; set; }
    }
}

