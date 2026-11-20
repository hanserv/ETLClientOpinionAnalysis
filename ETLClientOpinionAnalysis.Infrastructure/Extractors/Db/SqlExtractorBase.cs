using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Db
{
    public abstract class SqlExtractorBase<T> : IExtractor<T> where T : class
    {
        private readonly DatabaseSettings _settings;
        private readonly ILogger _logger;

        protected SqlExtractorBase(IOptions<DatabaseSettings> settings, ILogger logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public abstract string SourceName { get; }

        protected abstract string Query { get; }

        protected abstract T Map(SqlDataReader reader);

        public async Task<IReadOnlyList<T>> ExtractAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
            {
                throw new InvalidOperationException("La cadena de conexion de la base de datos no esta configurada.");
            }

            var records = new List<T>();

            await using var connection = new SqlConnection(_settings.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(Query, connection);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                records.Add(Map(reader));
            }

            _logger.LogInformation("Se leyeron {Count} registros de {Source} desde la base de datos", records.Count, SourceName);

            return records;
        }
    }
}
