namespace RendezVous.Domain.Entities;

public class ConfirmationToken : Entity
{
    public string Value { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
}
