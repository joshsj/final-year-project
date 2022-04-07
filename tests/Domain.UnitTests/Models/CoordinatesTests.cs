using NUnit.Framework;
using RendezVous.Domain.Models;

namespace RendezVous.Domain.UnitTests.Models;

public class CoordinatesTests
{
    
    /// <summary>1 Millimeter</summary>
    private const double Tolerance = 0.001; 
    
    [TestCaseSource(nameof(TestData))]
    public void CanMeasureDistance(Coordinates a, Coordinates b, Distance expected)
    {
        var distance = a.Distance(b);
        
        // tolerance required
        Assert.True(distance.Meters - expected.Meters < Tolerance);
    }

    /// <remarks>
    /// Coordinates sourced from Google Maps
    /// Results sourced from https://www.onlineconversion.com/map_greatcircle_distance.htm
    /// </remarks>
    public static IEnumerable<TestCaseData> TestData
    {
        get
        {
            yield return new TestCaseData(
                new Coordinates(0, 0),
                new Coordinates(0, 0),
                new Distance(0));
            
            // Sheffield Hallam libraries
            yield return new TestCaseData(
                new Coordinates(53.3717553, -1.4928765),
                new Coordinates(53.380264, -1.4675081),
                Distance.FromKilometers(1.9305366899397525));
            
            // London to Manchester
            yield return new TestCaseData(
                new Coordinates(51.5287718, -0.2416802),
                new Coordinates(53.4723272, -2.2935021),
                Distance.FromKilometers(256.87057377647767));
        }
    }
}
