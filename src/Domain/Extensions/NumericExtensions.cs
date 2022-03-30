namespace RendezVous.Domain.Extensions;

public static class NumericExtensions
{
    public static double ToRadians(this double d) => (Math.PI / 180) * d;
    public static double ToDegrees(this double d) => (180 / Math.PI) * d;
}
