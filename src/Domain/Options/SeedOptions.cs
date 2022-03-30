namespace RendezVous.Domain.Options;

public class SeedOptions
{
    public bool Enabled { get; set; }
    
    /// <summary>Employee to assign Jobs</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>Coordinates of Job locations</summary>
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
