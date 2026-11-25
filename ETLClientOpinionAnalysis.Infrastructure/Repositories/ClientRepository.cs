using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public class ClientRepository : StoredProcedureRepositoryBase<DimClient>
    {
        public ClientRepository(IOptions<WarehouseSettings> settings) : base(settings) { }

        public override string DimensionName => "Client";

        protected override string ProcedureName => "dim.usp_LoadClient";

        protected override SqlParameter[] GetParameters(DimClient item) => new[]
        {
            new SqlParameter("@ClientId", item.ClientId),
            new SqlParameter("@ClientName", item.ClientName),
            new SqlParameter("@Country", item.Country),
            new SqlParameter("@AgeRange", item.AgeRange)
        };
    }
}
