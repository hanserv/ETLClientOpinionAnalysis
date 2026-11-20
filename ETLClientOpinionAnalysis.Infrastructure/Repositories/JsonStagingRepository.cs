using System.Text.Encodings.Web;
using System.Text.Json;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Repositories
{
    public class JsonStagingRepository : IStagingRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly StagingSettings _settings;

        public JsonStagingRepository(IOptions<StagingSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SaveAsync<T>(string name, IReadOnlyList<T> records, CancellationToken cancellationToken = default) where T : class
        {
            Directory.CreateDirectory(_settings.OutputPath);

            var path = Path.Combine(_settings.OutputPath, $"{name}.json");

            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

            await JsonSerializer.SerializeAsync(stream, records, JsonOptions, cancellationToken);
        }

    }
}
