namespace ETLClientOpinionAnalysis.Core.Domain.Interfaces
{
    public interface IStagingRepository
    {
        Task<IReadOnlyList<T>> ReadAsync<T>(string name, CancellationToken cancellationToken = default)
            where T : class;

        Task SaveAsync<T>(string name, IReadOnlyList<T> records, CancellationToken cancellationToken = default)
            where T : class;
    }
}
