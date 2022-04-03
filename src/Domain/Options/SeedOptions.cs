namespace RendezVous.Domain.Options;

public class SeedOptions
{
    public bool Enabled { get; set; }
    
    public Guid YourEmployeeId { get; set; }
    public Guid OtherEmployeeId { get; set; }

    /// <summary>Coordinates of Job locations</summary>
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Coordinates Coordinates => new (Latitude, Longitude);
}
