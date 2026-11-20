using SocialCommentsApi.DTOs;
using SocialCommentsApi.Interfaces;

namespace SocialCommentsApi.Services
{
    public class SocialCommentService : ISocialCommentService
    {
        private const int MaxPageSize = 500;

        private readonly ISocialCommentRepository _socialCommentRepository;

        public SocialCommentService(ISocialCommentRepository socialCommentRepository)
        {
            _socialCommentRepository = socialCommentRepository;
        }

        public async Task<PagedResponse<SocialCommentDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

            var totalCount = await _socialCommentRepository.CountAsync(cancellationToken);
            var items = await _socialCommentRepository.GetPagedAsync((page - 1) * pageSize, pageSize, cancellationToken);

            return new PagedResponse<SocialCommentDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
        }
    }
}
