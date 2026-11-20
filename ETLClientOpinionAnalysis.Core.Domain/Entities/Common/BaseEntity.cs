namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Common
{
    public class BaseEntity<TKey> 
    {
        public required TKey Id { get; set; }
    }
}
