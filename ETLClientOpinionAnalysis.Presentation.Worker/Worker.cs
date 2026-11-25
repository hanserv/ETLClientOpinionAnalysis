using ETLClientOpinionAnalysis.Core.Application.Interfaces;

namespace ETLClientOpinionAnalysis.Presentation.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IExtractionService _extractionService;
        private readonly IDimensionLoadService _dimensionLoadService;
        private readonly IHostApplicationLifetime _lifetime;

        public Worker(IExtractionService extractionService, IDimensionLoadService dimensionLoadService,
            IHostApplicationLifetime lifetime)
        {
            _extractionService = extractionService;
            _dimensionLoadService = dimensionLoadService;
            _lifetime = lifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _extractionService.RunAsync(stoppingToken);
                await _dimensionLoadService.RunAsync(stoppingToken);
            }
            finally
            {
                _lifetime.StopApplication();
            }
        }
    }
}
