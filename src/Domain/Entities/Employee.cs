namespace RendezVous.Domain.Entities;

public class Employee : Entity
{
    public string ProviderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}