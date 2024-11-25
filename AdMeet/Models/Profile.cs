using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public class Profile
{
    public Profile(string idUser)
    {
        IdUser = idUser;
        City = "";
        Country = "";
        LastName = "";
        Name = "";
        ZipCode = "";
    }

    [Key] [MaxLength(100)] public string IdUser { get; set; }
    [MaxLength(100)] public string City { get; set; }
    [MaxLength(50)] public string Country { get; set; }
    [MaxLength(100)] public string LastName { get; set; }
    [MaxLength(50)] public string Name { get; set; }
    [MaxLength(10)] public string ZipCode { get; set; }
}