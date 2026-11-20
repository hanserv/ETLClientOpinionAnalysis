using ETLClientOpinionAnalysis.Core.Domain.Entities.Api;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Api
{
    internal class ApiPagedResponse
    {
        public int TotalPages { get; set; }
        public List<SocialComment> Items { get; set; } = [];
    }
}
