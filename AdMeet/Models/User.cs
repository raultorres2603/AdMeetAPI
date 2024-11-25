using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public class User
{
    public User(string email, string password)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        Password = password;
        Profile = new Profile(Id);
    }

    [MaxLength(100)] [Key] public string Id { get; set; }

    [MaxLength(100)] public string? Email { get; set; }

    [MaxLength(500)] public string? Password { get; set; }

    public Profile Profile { get; set; }

    public string HashPassword()
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(Password);
    }
}