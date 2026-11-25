namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions
{
    public class DimSentiment
    {
        public int SentimentKey { get; set; }
        public string SentimentName { get; set; } = string.Empty;
        public byte SortOrder { get; set; }
    }
}
