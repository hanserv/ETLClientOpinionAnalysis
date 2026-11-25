namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions
{
    public class DimClient
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string AgeRange { get; set; } = string.Empty;
    }
}
