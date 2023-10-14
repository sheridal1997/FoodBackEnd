namespace Food_Backend.Entity
{
    public class User : BaseEnntity<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? UserType { get; set; }

    }
    public enum UserType {

        Admin = 1,
        User = 2
    }
}
