namespace app.Domain.Entities
{
    public class AdminUser
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        private AdminUser() { }

        public AdminUser(Guid id, string username, string passwordHash)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty", nameof(username));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow; // التوقيت العالمي الموحد
        }
    }
}