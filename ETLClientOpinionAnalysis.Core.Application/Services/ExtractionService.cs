using System.Diagnostics;
using ETLClientOpinionAnalysis.Core.Application.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Api;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Csv;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Db;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ETLClientOpinionAnalysis.Core.Application.Services
{
    public class ExtractionService : IExtractionService
    {
        private readonly IExtractor<Survey> _surveyExtractor;
        private readonly IExtractor<WebReview> _webReviewExtractor;
        private readonly IExtractor<Client> _clientExtractor;
        private readonly IExtractor<Product> _productExtractor;
        private readonly IExtractor<SocialComment> _socialCommentExtractor;
        private readonly IStagingRepository _stagingRepository;
        private readonly ILogger<ExtractionService> _logger;

        public ExtractionService(IExtractor<Survey> surveyExtractor, IExtractor<WebReview> webReviewExtractor,
            IExtractor<Client> clientExtractor, IExtractor<Product> productExtractor,
            IExtractor<SocialComment> socialCommentExtractor, IStagingRepository staging, 
            ILogger<ExtractionService> logger)
        {
            _surveyExtractor = surveyExtractor;
            _webReviewExtractor = webReviewExtractor;
            _clientExtractor = clientExtractor;
            _productExtractor = productExtractor;
            _socialCommentExtractor = socialCommentExtractor;
            _stagingRepository = staging;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Inicia la extraccion");

            await Task.WhenAll(
                ExtractToStagingAsync(_surveyExtractor, cancellationToken),
                ExtractToStagingAsync(_webReviewExtractor, cancellationToken),
                ExtractToStagingAsync(_clientExtractor, cancellationToken),
                ExtractToStagingAsync(_productExtractor, cancellationToken),
                ExtractToStagingAsync(_socialCommentExtractor, cancellationToken));

            stopwatch.Stop();
            _logger.LogInformation("Extraccion terminada en {Elapsed} ms", stopwatch.ElapsedMilliseconds);
        }

        private async Task ExtractToStagingAsync<T>(IExtractor<T> extractor, CancellationToken cancellationToken) where T : class
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var records = await extractor.ExtractAsync(cancellationToken);
                await _stagingRepository.SaveAsync(extractor.SourceName, records, cancellationToken);
                
                stopwatch.Stop();

                _logger.LogInformation("{Source}: {Count} registros guardados en staging ({Elapsed} ms)",
                    extractor.SourceName, records.Count, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex, "Fallo la extraccion de {Source}", extractor.SourceName);
                throw;
            }
        }
    }
}
