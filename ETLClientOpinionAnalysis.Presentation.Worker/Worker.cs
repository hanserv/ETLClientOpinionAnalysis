using ETLClientOpinionAnalysis.Core.Application.Interfaces;

namespace ETLClientOpinionAnalysis.Presentation.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IExtractionService _extractionService;
        private readonly IHostApplicationLifetime _lifetime;

        public Worker(IExtractionService extractionService, IHostApplicationLifetime lifetime)
        {
            _extractionService = extractionService;
            _lifetime = lifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _extractionService.RunAsync(stoppingToken);
            }
            finally
            {
                _lifetime.StopApplication();
            }
        }
    }
}
