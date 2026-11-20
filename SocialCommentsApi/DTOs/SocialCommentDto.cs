namespace SocialCommentsApi.DTOs
{
    public class SocialCommentDto
    {
        public int Id { get; set; }
        public int? ClientId { get; set; } 
        public int ProductId { get; set; }
        public string Source { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
