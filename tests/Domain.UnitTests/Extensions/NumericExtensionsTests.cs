using NUnit.Framework;
using RendezVous.Domain.Extensions;

namespace RendezVous.Domain.UnitTests.Extensions;

public class NumericExtensionsTests
{
    [TestCaseSource(nameof(ToRadiansTestData))]
    public double CanConvertToRadians(double value) => NumericExtensions.ToRadians(value);    
    
    [TestCaseSource(nameof(ToDegreesTestData))]
    public double CanConvertToDegrees(double value) => NumericExtensions.ToDegrees(value);
    
    public static IEnumerable<TestCaseData> ToRadiansTestData
    {
        get
        {
            yield return new TestCaseData(0).Returns(0);
            yield return new TestCaseData(1).Returns(0.017453292519943295);
            yield return new TestCaseData(13).Returns(0.22689280275926285);
            yield return new TestCaseData(21).Returns(0.3665191429188092);
            yield return new TestCaseData(34).Returns(0.5934119456780721);
            yield return new TestCaseData(55).Returns(0.9599310885968813);
            yield return new TestCaseData(89).Returns(1.5533430342749532);
            yield return new TestCaseData(144).Returns(2.5132741228718345);
            
            yield return new TestCaseData(-0).Returns(0);
            yield return new TestCaseData(-1).Returns(-0.017453292519943295);
            yield return new TestCaseData(-13).Returns(-0.22689280275926285); 
            yield return new TestCaseData(-21).Returns(-0.3665191429188092);
            yield return new TestCaseData(-34).Returns(-0.5934119456780721);
            yield return new TestCaseData(-55).Returns(-0.9599310885968813);
            yield return new TestCaseData(-89).Returns(-1.5533430342749532);
            yield return new TestCaseData(-144).Returns(-2.5132741228718345);
        }
    }
    
    public static IEnumerable<TestCaseData> ToDegreesTestData
    {
        get
        {
            yield return new TestCaseData(0).Returns(0);
            yield return new TestCaseData(0.01).Returns(0.5729577951308232);
            yield return new TestCaseData(0.13).Returns(7.448451336700702);
            yield return new TestCaseData(0.01).Returns(0.5729577951308232);
            yield return new TestCaseData(0.34).Returns(19.48056503444799);
            yield return new TestCaseData(0.55).Returns(31.51267873219528);
            yield return new TestCaseData(0.89).Returns(50.99324376664327);
            yield return new TestCaseData(1.44).Returns(82.50592249883854);
            
            yield return new TestCaseData(-0).Returns(0);
            yield return new TestCaseData(-0.01).Returns(-0.5729577951308232);
            yield return new TestCaseData(-0.13).Returns(-7.448451336700702);
            yield return new TestCaseData(-0.21).Returns(-12.032113697747288);
            yield return new TestCaseData(-0.34).Returns(-19.48056503444799);
            yield return new TestCaseData(-0.55).Returns(-31.51267873219528);
            yield return new TestCaseData(-0.89).Returns(-50.99324376664327);
            yield return new TestCaseData(-1.44).Returns(-82.50592249883854);
        }
    }
}
