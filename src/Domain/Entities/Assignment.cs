namespace RendezVous.Domain.Entities;

public class Assignment : Entity
{
    public string Notes { get; set; } = null!;
    
    public Guid JobId { get; set; }
    public Job Job { get; set; } = null!;
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public ICollection<Clock> Clocks { get; } = new List<Clock>();

    public Clock? ClockIn => Clocks.SingleOrDefault(x => x.Type == ClockType.In);
    public Clock? ClockOut => Clocks.SingleOrDefault(x => x.Type == ClockType.Out);
}
