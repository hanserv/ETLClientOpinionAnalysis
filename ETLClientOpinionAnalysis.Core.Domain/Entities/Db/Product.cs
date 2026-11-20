using ETLClientOpinionAnalysis.Core.Domain.Entities.Common;

namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Db
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
