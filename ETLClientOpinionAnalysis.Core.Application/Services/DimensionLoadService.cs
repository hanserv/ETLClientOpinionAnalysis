using System.Globalization;
using ETLClientOpinionAnalysis.Core.Application.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Api;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Csv;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Db;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETLClientOpinionAnalysis.Core.Application.Services
{
    public class DimensionLoadService : IDimensionLoadService
    {
        private readonly IDimensionRepository<DimSentiment> _sentimentRepository;
        private readonly IDimensionRepository<DimSource> _sourceRepository;
        private readonly IDimensionRepository<DimDate> _dateRepository;
        private readonly IDimensionRepository<DimProduct> _productRepository;
        private readonly IDimensionRepository<DimClient> _clientRepository;
        private readonly IStagingRepository _staging;
        private readonly ILogger<DimensionLoadService> _logger;

        public DimensionLoadService(IDimensionRepository<DimSentiment> sentimentRepository, IDimensionRepository<DimSource> sourceRepository,
            IDimensionRepository<DimDate> dateRepository, IDimensionRepository<DimProduct> productRepository,
            IDimensionRepository<DimClient> clientRepository, IStagingRepository staging, ILogger<DimensionLoadService> logger)
        {
            _sentimentRepository = sentimentRepository;
            _sourceRepository = sourceRepository;
            _dateRepository = dateRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _staging = staging;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Inicia la carga de dimensiones");

            var sentiments = new List<DimSentiment>
            {
                new() { SentimentKey = -1, SentimentName = "No clasificado", SortOrder = 4 },
                new() { SentimentKey =  1, SentimentName = "Positivo", SortOrder = 1 },
                new() { SentimentKey =  2, SentimentName = "Neutro", SortOrder = 2 },
                new() { SentimentKey =  3, SentimentName = "Negativo", SortOrder = 3 }
            };

            var sources = new List<DimSource>
            {
                new() { Channel = "Encuesta", Platform = "N/A", SourceType = "CSV" },
                new() { Channel = "Reseña web", Platform = "N/A", SourceType = "BD relacional" },
                new() { Channel = "Red social", Platform = "Instagram", SourceType = "API REST" },
                new() { Channel = "Red social", Platform = "Facebook", SourceType = "API REST" },
                new() { Channel = "Red social", Platform = "Twitter", SourceType = "API REST" }
            };

            await LoadAsync(_sentimentRepository, sentiments, cancellationToken);
            await LoadAsync(_sourceRepository, sources, cancellationToken);

            var dates = await BuildDatesAsync(cancellationToken);
            await LoadAsync(_dateRepository, dates, cancellationToken);

            var products = await BuildProductsAsync(cancellationToken);
            await LoadAsync(_productRepository, products, cancellationToken);

            var clients = await BuildClientsAsync(cancellationToken);
            await LoadAsync(_clientRepository, clients, cancellationToken);

            _logger.LogInformation("Carga de dimensiones terminada");
        }

        #region Private Methods

        private async Task LoadAsync<T>(IDimensionRepository<T> repository, IEnumerable<T> items,
            CancellationToken cancellationToken) where T : class
        {
            foreach (var item in items)
            {
                var result = await repository.LoadAsync(item, cancellationToken);
                _logger.LogInformation("{Dimension}: {Result}", repository.DimensionName, result);
            }
        }

        private async Task<List<DimDate>> BuildDatesAsync(CancellationToken cancellationToken)
        {
            var surveys = await _staging.ReadAsync<Survey>("surveys", cancellationToken);
            var webReviews = await _staging.ReadAsync<WebReview>("web_reviews", cancellationToken);
            var socialComments = await _staging.ReadAsync<SocialComment>("social_comments", cancellationToken);

            var dates = new List<DateOnly>();
            var invalid = 0;

            foreach (var survey in surveys)
            {
                if (DateOnly.TryParseExact(survey.Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var date))
                {
                    dates.Add(date);
                }
                else
                {
                    invalid++;
                }
            }

            dates.AddRange(webReviews.Select(r => r.Date));
            dates.AddRange(socialComments.Select(c => c.Date));

            if (invalid > 0)
            {
                _logger.LogWarning("{Count} encuestas tienen una fecha que no se pudo leer y se ignoraron", invalid);
            }

            if (dates.Count == 0)
            {
                throw new InvalidOperationException("No hay fechas en el staging para generar la dimension de fechas.");
            }

            var start = dates.Min();
            var end = dates.Max();

            _logger.LogInformation("Rango de fechas del staging: {Start} a {End}", start, end);

            return GenerateDates(start, end);
        }

        private static List<DimDate> GenerateDates(DateOnly start, DateOnly end)
        {
            var dates = new List<DimDate>();

            for (var day = start; day <= end; day = day.AddDays(1))
            {
                dates.Add(new DimDate { FullDate = day });
            }

            return dates;
        }

        private async Task<List<DimProduct>> BuildProductsAsync(CancellationToken cancellationToken)
        {
            var products = await _staging.ReadAsync<Product>("products", cancellationToken);

            return products
                .Select(p => new DimProduct
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    CategoryName = p.CategoryName
                })
                .ToList();
        }

        private async Task<List<DimClient>> BuildClientsAsync(CancellationToken cancellationToken)
        {
            var clients = await _staging.ReadAsync<Client>("clients", cancellationToken);

            var result = new List<DimClient>
            {
                new() { ClientId = "N/A", ClientName = "Desconocido", Country = "Desconocido", AgeRange = "Desconocido" }
            };

            result.AddRange(clients.Select(c => new DimClient
            {
                ClientId = c.Id.ToString(),
                ClientName = c.Name,
                Country = c.CountryName,
                AgeRange = GetAgeRange(c.BirthDate)
            }));

            return result;
        }

        private static string GetAgeRange(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;

            return age switch
            {
                < 18 => "Menor de 18",
                < 25 => "18-24",
                < 35 => "25-34",
                < 45 => "35-44",
                < 55 => "45-54",
                _ => "55+"
            };
        }
        #endregion
    }
}
