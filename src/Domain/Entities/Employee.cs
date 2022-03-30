namespace RendezVous.Domain.Entities;

public class Employee : Entity
{
    public string ProviderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public ICollection<Assignment> Assignments { get; } = new List<Assignment>();
}
