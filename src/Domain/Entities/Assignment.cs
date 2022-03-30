namespace RendezVous.Domain.Entities;

public class Assignment : Entity
{
    public Guid JobId { get; set; }
    public Job Job { get; set; } = null!;
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public ICollection<Clock> Clocks { get; } = new List<Clock>();
}
