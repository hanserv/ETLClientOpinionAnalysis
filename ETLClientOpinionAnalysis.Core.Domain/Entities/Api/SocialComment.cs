using ETLClientOpinionAnalysis.Core.Domain.Entities.Common;

namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Api
{
    public class SocialComment : BaseEntity<int>
    {
        public int? ClientId { get; set; }
        public int ProductId { get; set; }
        public string Source { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
