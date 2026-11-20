using System.Data;
using Microsoft.Data.SqlClient;
using SocialCommentsApi.DTOs;
using SocialCommentsApi.Interfaces;

namespace SocialCommentsApi.Repositories
{
    public class SocialCommentRepository : ISocialCommentRepository
    {
        private const string CountQuery = "SELECT COUNT(*) FROM dbo.SocialComments;";

        private const string PageQuery = @"
            SELECT sc.Id, sc.ClientId, sc.ProductId, s.Name AS Source, sc.[Date], sc.Comment
            FROM dbo.SocialComments sc
            JOIN dbo.SocialCommentSources s ON s.Id = sc.SocialCommentSourceId
            ORDER BY sc.Id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        private readonly string _connectionString;

        public SocialCommentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Falta la cadena de conexion.");
        }


        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(CountQuery, connection);
            var result = await command.ExecuteScalarAsync(cancellationToken);

            return Convert.ToInt32(result);
        }

        public async Task<IReadOnlyList<SocialCommentDto>> GetPagedAsync(int offset, int pageSize, CancellationToken cancellationToken)
        {
            var comments = new List<SocialCommentDto>();

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(PageQuery, connection);
            command.Parameters.Add("@Offset", SqlDbType.Int).Value = offset;
            command.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                comments.Add(new SocialCommentDto
                {
                    Id = reader.GetInt32(0),
                    ClientId = reader.IsDBNull(1) ? null : reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    Source = reader.GetString(3),
                    Date = reader.GetFieldValue<DateOnly>(4),
                    Comment = reader.GetString(5)
                });
            }

            return comments;
        }
    }
}
