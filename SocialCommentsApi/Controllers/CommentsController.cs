using Microsoft.AspNetCore.Mvc;
using SocialCommentsApi.DTOs;
using SocialCommentsApi.Interfaces;

namespace SocialCommentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ISocialCommentService _socialCommentService;

        public CommentsController(ISocialCommentService socialCommentService)
        {
            _socialCommentService = socialCommentService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<SocialCommentDto>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 100, CancellationToken cancellationToken = default)
        {
            var result = await _socialCommentService.GetPagedAsync(page, pageSize, cancellationToken);
            return Ok(result);
        }
    }
}
