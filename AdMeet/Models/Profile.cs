using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public class Profile(string idUser, string city, string country, string lastName, string name, string zipCode)
{
    public Profile(string idUser) : this(idUser, "", "", "", "", "")
    {
    }

    [Key] [MaxLength(100)] public string IdUser { get; set; } = idUser;
    [MaxLength(100)] public string City { get; set; } = city;
    [MaxLength(50)] public string Country { get; set; } = country;
    [MaxLength(100)] public string LastName { get; set; } = lastName;
    [MaxLength(50)] public string Name { get; set; } = name;
    [MaxLength(10)] public string ZipCode { get; set; } = zipCode;
}