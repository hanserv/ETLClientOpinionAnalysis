using ETLClientOpinionAnalysis.Core.Domain.Entities.Common;

namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Db
{
    public class WebReview : BaseEntity<int>
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public DateOnly Date { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
