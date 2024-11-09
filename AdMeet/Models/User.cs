using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public class User
{
    public User(string email, string password)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    [MaxLength(100)] public string Id { get; set; }

    [MaxLength(100)] public string? Email { get; set; }

    [MaxLength(2500)] public string? Password { get; set; }
}