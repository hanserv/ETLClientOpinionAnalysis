namespace ETLClientOpinionAnalysis.Core.Domain.Interfaces
{
    public interface IDimensionRepository<T> where T : class
    {
        string DimensionName { get; }

        Task<string> LoadAsync(T item, CancellationToken cancellationToken = default);
    }
}
