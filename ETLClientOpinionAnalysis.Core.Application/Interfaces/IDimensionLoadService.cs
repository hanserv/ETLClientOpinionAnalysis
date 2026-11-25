namespace ETLClientOpinionAnalysis.Core.Application.Interfaces
{
    public interface IDimensionLoadService
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
