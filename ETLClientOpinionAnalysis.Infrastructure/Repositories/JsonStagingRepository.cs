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

        public async Task<IReadOnlyList<T>> ReadAsync<T>(string name, CancellationToken cancellationToken = default) where T : class
        {
            var path = Path.Combine(_settings.OutputPath, $"{name}.json");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"No existe el archivo de staging '{path}'. Ejecuta primero la extraccion.");
            }

            await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true);

            var records = await JsonSerializer.DeserializeAsync<List<T>>(stream, JsonOptions, cancellationToken);

            return records ?? new List<T>();
        }
    }
}
