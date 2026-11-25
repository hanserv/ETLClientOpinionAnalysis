
using ETLClientOpinionAnalysis.Core.Domain.Entities.Db;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Db
{
    public class ClientExtractor : SqlExtractorBase<Client>
    {
        public ClientExtractor(IOptions<DatabaseSettings> settings, ILogger<ClientExtractor> logger)
            : base(settings, logger) { }

        public override string SourceName => "clients";

        protected override string Query => @"
            SELECT c.Id, c.Name, co.Name AS CountryName, c.BirthDate
            FROM dbo.Clients c
            JOIN dbo.Countries co ON co.Id = c.CountryId;";

        protected override Client Map(SqlDataReader reader) => new()
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            CountryName = reader.GetString(2),
            BirthDate = reader.GetFieldValue<DateOnly>(3)
        };
    }
}
