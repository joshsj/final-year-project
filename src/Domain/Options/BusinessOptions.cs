namespace RendezVous.Domain.Options;

public class BusinessOptions
{
    public TimeSpan EarlyClockInThreshold { get; set; }
    public TimeSpan LateClockOutThreshold { get; set; }
    
    public TimeSpan ConfirmationTokenWindow { get; set; }
}
