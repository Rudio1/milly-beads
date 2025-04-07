namespace MillyBeads.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Salt { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
    }
} 