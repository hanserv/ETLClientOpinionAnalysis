using System.Data;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public class DateRepository : StoredProcedureRepositoryBase<DimDate>
    {
        public DateRepository(IOptions<WarehouseSettings> settings) : base(settings) { }

        public override string DimensionName => "Date";

        protected override string ProcedureName => "dim.usp_LoadDate";

        protected override SqlParameter[] GetParameters(DimDate item) => new[]
        {
            new SqlParameter("@FullDate", SqlDbType.Date)
            {
                Value = item.FullDate.ToDateTime(TimeOnly.MinValue)
            }
        };
    }
}
