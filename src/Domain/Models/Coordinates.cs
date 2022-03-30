using RendezVous.Domain.Extensions;
using static System.Math;

namespace RendezVous.Domain.Models;

public record Coordinates(double Latitude, double Longitude)
{
    /// <remarks>Sourced from https://nssdc.gsfc.nasa.gov/planetary/factsheet/earthfact.html</remarks>
    public static readonly Distance EarthRadius = Models.Distance.FromKilometers(6371);
    
    public Distance Distance(Coordinates other)
    {
        var left = new Coordinates(this.Latitude.ToRadians(), this.Longitude.ToRadians());
        var right = new Coordinates(other.Latitude.ToRadians(), other.Longitude.ToRadians());

        return Haversine(left, right, EarthRadius);
    }

    /// <summary>Implementation of the Haversine formula. All coordinates must be in radians.</summary>
    /// <remarks>Adapted from https://www.movable-type.co.uk/scripts/latlong.html</remarks>
    private static Distance Haversine(Coordinates left, Coordinates right, Distance radius)
    {
        double Square(double d) => Pow(d, 2);

        var diffLat = (right.Latitude - left.Latitude);
        var diffLong = (right.Longitude - left.Longitude);

        // square of half the chord length between the points
        var a = Square(Sin(diffLat / 2)) + Cos(left.Latitude) * Cos(right.Latitude) * Square(Sin(diffLong / 2));

        // angular distance
        var c = Atan2(Sqrt(a), Sqrt(1 - a)) * 2;
        
        // apply scale using radius
        return new Distance(radius.Meters * c);
    }
}
