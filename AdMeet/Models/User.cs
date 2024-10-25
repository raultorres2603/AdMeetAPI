using System.ComponentModel.DataAnnotations;
using AdMeet.Contexts;

namespace AdMeet.Models;

public class User
{
    public User(string email, string password)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        Password = password;
    }

    public AppDbContext Context { get; set; }

    [MaxLength(100)] public string Id { get; set; }

    [MaxLength(100)] public string? Email { get; set; }

    [MaxLength(100)] public string? Password { get; set; }
}