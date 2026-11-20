using SocialCommentsApi.DTOs;

namespace SocialCommentsApi.Interfaces
{
    public interface ISocialCommentRepository
    {
        Task<int> CountAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<SocialCommentDto>> GetPagedAsync(int offset, int pageSize, CancellationToken cancellationToken);
    }
}
