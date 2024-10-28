using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public interface IUser
{
    public string Id { get; }
    public string? Email { get; }
    public string? Password { get; }
    
}

public class User: IUser
{
    public User(string email, string password)
    {
        Id = Guid.NewGuid().ToString();
        Email = email;
        Password = password;
    }

    [MaxLength(100)] public string Id { get; set; }

    [MaxLength(100)] public string? Email { get; set; }

    [MaxLength(100)] public string? Password { get; set; }
}