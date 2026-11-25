namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions
{
    public class DimSource
    {
        public string Channel { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string SourceType { get; set; } = string.Empty;
    }
}
