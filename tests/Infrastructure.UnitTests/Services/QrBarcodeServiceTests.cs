using NUnit.Framework;
using RendezVous.Infrastructure.Services;

namespace RendezVous.Infrastructure.UnitTests.Services;

public class QrBarcodeServiceTests
{
    [Test]
    public void CanCreateSvg()
    {
        // Arrange
        const string data = "Hello, World!";
        
        // Act
        var sut = new QrBarcodeService();

        var result = sut.CreateSvg(data);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Does.StartWith("<svg"));
    }
}
