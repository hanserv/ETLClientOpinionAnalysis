using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public class SentimentRepository : StoredProcedureRepositoryBase<DimSentiment>
    {
        public SentimentRepository(IOptions<WarehouseSettings> settings) : base(settings) { }

        public override string DimensionName => "Sentiment";

        protected override string ProcedureName => "dim.usp_LoadSentiment";

        protected override SqlParameter[] GetParameters(DimSentiment item) => new[]
        {
            new SqlParameter("@SentimentKey", item.SentimentKey),
            new SqlParameter("@SentimentName", item.SentimentName),
            new SqlParameter("@SortOrder", item.SortOrder)
        };
    }
}
