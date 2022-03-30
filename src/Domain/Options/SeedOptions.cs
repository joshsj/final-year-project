namespace RendezVous.Domain.Options;

public class SeedOptions
{
    /// <summary>Employee to assign Jobs</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>Coordinates of Job locations</summary>
    public Coordinates Coordinates { get; set; } = null!;
}
