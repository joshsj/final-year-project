namespace RendezVous.Domain.Entities;

public class ConfirmationToken : Entity
{
    public string Value { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    
    public Guid ConfirmeeAssignmentId { get; set; }
    public Assignment ConfirmeeAssignment { get; set; } = null!;
    
    public Guid ConfirmerAssignmentId { get; set; }
    public Assignment ConfirmerAssignment { get; set; } = null!;
}
