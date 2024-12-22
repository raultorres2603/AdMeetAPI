using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdMeet.Models;

public class Profile(
    string idUser,
    string city = "",
    string country = "",
    string lastName = "",
    string name = "",
    string zipCode = "",
    DateOnly birthday = new(),
    string gender = "",
    string preferences = ""
)
{
    [ForeignKey("IdUser")]
    [Key]
    [MaxLength(100)]
    public string IdUser { get; set; } = idUser;

    [MaxLength(100)] public string City { get; set; } = city;
    [MaxLength(50)] public string Country { get; set; } = country;
    [MaxLength(100)] public string LastName { get; set; } = lastName;
    [MaxLength(50)] public string Name { get; set; } = name;
    [MaxLength(10)] public string ZipCode { get; set; } = zipCode;

    public DateOnly Birthday { get; set; } = birthday;

    [MaxLength(1)] public string Gender { get; set; } = gender;

    [MaxLength(100)] public string Preferences { get; set; } = preferences;
}