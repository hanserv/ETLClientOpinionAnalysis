namespace ETLClientOpinionAnalysis.Core.Domain.Interfaces
{
    public interface IExtractor<T> where T : class
    {
        string SourceName { get; }
        Task<IReadOnlyList<T>> ExtractAsync(CancellationToken cancellationToken = default);
    }
}
