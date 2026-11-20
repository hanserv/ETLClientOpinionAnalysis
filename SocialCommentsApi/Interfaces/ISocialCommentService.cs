using SocialCommentsApi.DTOs;

namespace SocialCommentsApi.Interfaces
{
    public interface ISocialCommentService
    {
        Task<PagedResponse<SocialCommentDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    }
}
