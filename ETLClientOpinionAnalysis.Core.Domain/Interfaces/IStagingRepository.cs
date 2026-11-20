namespace ETLClientOpinionAnalysis.Core.Domain.Interfaces
{
    public interface IStagingRepository
    {
        Task SaveAsync<T>(string name, IReadOnlyList<T> records, CancellationToken cancellationToken = default)
            where T : class;
    }
}
