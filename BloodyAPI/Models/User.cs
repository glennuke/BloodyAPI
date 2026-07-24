namespace BloodyAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string PasswordHash { get; set; } = "";

        public bool IsSubscribed { get; set; } = false;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;
    }
}
