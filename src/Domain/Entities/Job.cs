using RendezVous.Domain.Models;

namespace RendezVous.Domain.Entities;

public class Job : Entity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;
    
    public ICollection<Assignment> Assignments { get; } = new List<Assignment>();
}
