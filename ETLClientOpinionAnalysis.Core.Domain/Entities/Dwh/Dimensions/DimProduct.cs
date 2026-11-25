namespace ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions
{
    public class DimProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
