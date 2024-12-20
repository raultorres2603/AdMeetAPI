using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public class Category(string name, string icon)
{
    [Key] public Guid Id { get; set; } = new();

    [MaxLength(100)] public string Name { get; set; } = name;

    [MaxLength(100)] public string Icon { get; set; } = icon;
}