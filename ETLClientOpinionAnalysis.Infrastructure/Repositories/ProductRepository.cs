using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public class ProductRepository : StoredProcedureRepositoryBase<DimProduct>
    {
        public ProductRepository(IOptions<WarehouseSettings> settings) : base(settings) { }

        public override string DimensionName => "Product";

        protected override string ProcedureName => "dim.usp_LoadProduct";

        protected override SqlParameter[] GetParameters(DimProduct item) => new[]
        {
            new SqlParameter("@ProductId", item.ProductId),
            new SqlParameter("@ProductName", item.ProductName),
            new SqlParameter("@CategoryName", item.CategoryName)
        };
    }
}
