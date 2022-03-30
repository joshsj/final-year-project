using RendezVous.Domain.Enums;

namespace RendezVous.Domain.Entities;

public class Clock : Entity
{
    public ClockType Type { get; set; }
    public DateTime ExpectedAt { get; set; }
    public DateTime? ActualAt { get; set; }

    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    
    public Guid? ParentId { get; set; }
    public Clock? Parent { get; set; }

    public ICollection<Clock> Children { get; } = new List<Clock>();
}
