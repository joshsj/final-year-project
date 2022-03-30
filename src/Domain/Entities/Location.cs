using RendezVous.Domain.Models;

namespace RendezVous.Domain.Entities;

public class Location : Entity
{
    public string Title { get; set; } = null!;
    public Coordinates Coordinates { get; set; } = null!;
    public Distance Radius { get; set; } = null!;

    public ICollection<Job> Jobs { get; } = new List<Job>();
}
