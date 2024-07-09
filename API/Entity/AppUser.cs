namespace API.Entity
{
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required byte[] PasswordHas { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
