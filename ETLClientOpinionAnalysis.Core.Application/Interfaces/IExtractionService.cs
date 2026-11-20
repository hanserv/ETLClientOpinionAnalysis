namespace ETLClientOpinionAnalysis.Core.Application.Interfaces
{
    public interface IExtractionService
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
