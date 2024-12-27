using System.ComponentModel.DataAnnotations;

namespace AdMeet.Models;

public class Kpi
{
    public Kpi(string endPoint)
    {
        EndPoint = endPoint;
        Id = Guid.NewGuid();
        EnteredOn = DateTime.Now;
    }

    [MaxLength(100)] [Key] public Guid Id { get; init; }
    [MaxLength(50)] public string EndPoint { get; init; }
    [MaxLength(150)] public DateTime EnteredOn { get; init; }
}