using System.Text;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace HtmlToPdfApi
{
    public interface IHtmlToPdfService
    {
        byte[] Convert(ConvertRequest request);
    }

    public class HtmlToPdfService : IHtmlToPdfService
    {
        private static readonly byte[] _emptyByteArray = Array.Empty<byte>();

        private readonly IConverter _pdfConverter;

        private static GlobalSettings _globalSettings = new()
        {
            Outline = false,
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4
        };

        private static MarginSettings _defaultMargins = new()
        {
            Unit = WkHtmlToPdfDotNet.Unit.Centimeters,
            Left = 1.5,
            Right = 1.5
        };

        public HtmlToPdfService(IConverter pdfConverter)
        {
            _pdfConverter = pdfConverter;
        }

        public byte[] Convert(ConvertRequest request)
        {
            if (request is null || request.HtmlContentBase64 is null)
                return _emptyByteArray;

            // Default configurations for PDF file
            var config = _globalSettings;
            config.DocumentTitle = request.DocTitle;
            config.Orientation = (Orientation)request.Orientation;
            config.Margins = _defaultMargins;

            // Custom margins
            if (request.CustomMargins is not null)
            {
                config.Margins.Unit = (Unit)request.CustomMargins.Value.Unit;
                config.Margins.Left = request.CustomMargins.Value.Left;
                config.Margins.Right = request.CustomMargins.Value.Right;
                config.Margins.Top = request.CustomMargins.Value.Top;
                config.Margins.Bottom = request.CustomMargins.Value.Bottom;
            }

            // Custom footer
            FooterSettings fs = null;
            if (request.CustomFooter is not null)
            {
                fs = new FooterSettings
                {
                    Left = request.CustomFooter.TextLeft,
                    Right = request.CustomFooter.TextRight,
                    Center = request.CustomFooter.TextoCenter,
                    Line = request.CustomFooter.ShowLineAbove,
                    FontSize = request.CustomFooter.FontSize,
                    FontName = request.CustomFooter.FontName
                };
            }

            // TODO: Custom header

            // Convert to PDF
            var htmlContent = Encoding.UTF8.GetString(System.Convert.FromBase64String(request.HtmlContentBase64));
            var pdfDoc = new HtmlToPdfDocument()
            {
                GlobalSettings = config,
                Objects = {
                        new ObjectSettings()
                        {
                            HtmlContent = htmlContent,
                            WebSettings = { DefaultEncoding = "utf-8", EnableJavascript = false },
                            FooterSettings = fs
                        }
                    }
            };

            return _pdfConverter.Convert(pdfDoc);
        }
    }
}