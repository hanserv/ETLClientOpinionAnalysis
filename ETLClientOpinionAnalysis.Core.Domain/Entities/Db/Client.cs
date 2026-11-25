using ETLClientOpinionAnalysis.Core.Domain.Entities.Common;

namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Db
{
    public class Client : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
    }
}
