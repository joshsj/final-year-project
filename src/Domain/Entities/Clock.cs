using RendezVous.Domain.Enums;

namespace RendezVous.Domain.Entities;

public class Clock : Entity
{
    public ClockType Type { get; set; }
    public DateTime At { get; set; }
    public Coordinates Coordinates { get; set; } = null!;

    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    
    public Guid? ParentId { get; set; }
    public Clock? Parent { get; set; }

    public ICollection<Clock> Children { get; } = new List<Clock>();
}
