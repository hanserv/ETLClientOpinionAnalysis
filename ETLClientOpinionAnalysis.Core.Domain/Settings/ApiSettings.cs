namespace ETLClientOpinionAnalysis.Core.Domain.Settings
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public int PageSize { get; set; } = 100;
    }
}
