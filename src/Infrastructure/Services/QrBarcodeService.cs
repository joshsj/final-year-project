using System.Drawing;
using RendezVous.Application.Common.Interfaces;
using QRCoder;

namespace RendezVous.Infrastructure.Services;

public class QrBarcodeService : IBarcodeService
{
    private static readonly Size _size = new Size(500, 500);
    
    public string CreateSvg(string data)
    {
        // configured as per documentation
        
        var codeData = new QRCodeGenerator()
            .CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        
        return new SvgQRCode(codeData).GetGraphic(
            _size,
            true,
            SvgQRCode.SizingMode.ViewBoxAttribute);
    }
}
