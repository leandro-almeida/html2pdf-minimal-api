namespace HtmlToPdfApi
{
    public enum PaperOrientation
    {
        Landscape,
        Portrait
    }

    public enum UnitOfMeasurement
    {
        Inches,
        Millimeters,
        Centimeters
    }

    public record struct MarginsConfig(UnitOfMeasurement Unit, double Top, double Right, double Bottom, double Left);

    public record FooterConfig
    {
        public int FontSize { get; init; } = 8;
        public string FontName { get; init; } = "sans-serif";
        public string TextLeft { get; init; } = string.Empty;
        public string TextRight { get; init; } = string.Empty;
        public string TextCenter { get; init; } = string.Empty;
        public bool ShowLineAbove { get; init; }
    }

    public record ConvertRequest
    {
        /// <summary>
        /// Conteudo HTML a ser convertido, em formato Base64
        /// </summary>
        public string? HtmlContentBase64 { get; init; }

        /// <summary>
        /// Titulo do documento PDF, visualizado nas propriedades do PDF).
        /// </summary>
        public string DocTitle { get; init; } = "Document";

        public PaperOrientation Orientation { get; init; } = PaperOrientation.Portrait;
        public MarginsConfig? CustomMargins { get; init; }
        public FooterConfig? CustomFooter { get; init; }
    }
}