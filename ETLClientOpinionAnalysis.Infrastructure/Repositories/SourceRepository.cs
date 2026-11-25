using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public class SourceRepository : StoredProcedureRepositoryBase<DimSource>
    {
        public SourceRepository(IOptions<WarehouseSettings> settings) : base(settings) { }

        public override string DimensionName => "Source";

        protected override string ProcedureName => "dim.usp_LoadSource";

        protected override SqlParameter[] GetParameters(DimSource item) => new[]
        {
            new SqlParameter("@Channel", item.Channel),
            new SqlParameter("@Platform", item.Platform),
            new SqlParameter("@SourceType", item.SourceType)
        };
    }
}
