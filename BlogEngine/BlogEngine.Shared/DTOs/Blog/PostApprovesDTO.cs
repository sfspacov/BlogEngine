namespace BlogEngine.Shared.DTOs.Blog
{
    public class PostApprovesDTO
    {
        public string EditorsReview { get; set; }
        public PostStatusEnum Status { get; set; }
    }
}