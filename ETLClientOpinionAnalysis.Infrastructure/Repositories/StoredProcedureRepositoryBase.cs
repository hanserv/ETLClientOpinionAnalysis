using System.Data;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public abstract class StoredProcedureRepositoryBase<T> : IDimensionRepository<T> where T : class
    {
        private readonly WarehouseSettings _settings;

        protected StoredProcedureRepositoryBase(IOptions<WarehouseSettings> settings)
        {
            _settings = settings.Value;
        }

        public abstract string DimensionName { get; }

        protected abstract string ProcedureName { get; }

        protected abstract SqlParameter[] GetParameters(T item);

        public async Task<string> LoadAsync(T item, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
            {
                throw new InvalidOperationException("La cadena de conexion del warehouse no esta configurada.");
            }

            await using var connection = new SqlConnection(_settings.ConnectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(ProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddRange(GetParameters(item));

            var result = new SqlParameter("@p_result", SqlDbType.VarChar, -1)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(result);

            await command.ExecuteNonQueryAsync(cancellationToken);

            return result.Value as string ?? string.Empty;
        }
    }
}
