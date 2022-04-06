using NUnit.Framework;
using RendezVous.Domain.Models;

namespace RendezVous.Domain.UnitTests.Models;

public class DistanceTests
{
    [TestCaseSource(nameof(TestData))]
    public Distance CanConvertFromKilometers(double kilometers)
        => Distance.FromKilometers(kilometers);

    public static IEnumerable<TestCaseData> TestData
    {
        get
        {
            yield return new TestCaseData(0d).Returns(new Distance(0));
            yield return new TestCaseData(0d).Returns(new Distance(0));
            yield return new TestCaseData(0.001d).Returns(new Distance(1));
            yield return new TestCaseData(1d).Returns(new Distance(1000));
            yield return new TestCaseData(1.234d).Returns(new Distance(1234));
            
            yield return new TestCaseData(-0d).Returns(new Distance(0));
            yield return new TestCaseData(-0d).Returns(new Distance(0));
            yield return new TestCaseData(-0.001d).Returns(new Distance(-1));
            yield return new TestCaseData(-1d).Returns(new Distance(-1000));
            yield return new TestCaseData(-1.234d).Returns(new Distance(-1234));
        }
    }

}
