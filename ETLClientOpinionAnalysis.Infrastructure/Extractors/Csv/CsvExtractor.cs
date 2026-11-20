using System.Globalization;
using CsvHelper;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Csv;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Csv
{
    public class CsvExtractor : IExtractor<Survey>
    {
        private readonly CsvSettings _settings;
        private readonly ILogger<CsvExtractor> _logger;

        public CsvExtractor(IOptions<CsvSettings> settings, ILogger<CsvExtractor> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public string SourceName => "surveys";

        public async Task<IReadOnlyList<Survey>> ExtractAsync(CancellationToken cancellationToken = default)
        {
            var path = _settings.SurveysPath;

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("La ruta del archivo no puede ser nula o vacía.");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"El archivo en '{path}' no fue encontrado.");
            }

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<SurveyMap>();

            var records = new List<Survey>();

            await foreach (var record in csv.GetRecordsAsync<Survey>(cancellationToken))
            {
                records.Add(record);
            }

            _logger.LogInformation("Se leyeron {Count} encuestas desde {Path}", records.Count, path);

            return records;
        }
    }
}
