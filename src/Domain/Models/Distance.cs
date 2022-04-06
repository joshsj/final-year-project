namespace RendezVous.Domain.Models;

public record Distance(double Meters)
{
    public static Distance FromKilometers(double kilometers) => new (kilometers * 1000);
}
