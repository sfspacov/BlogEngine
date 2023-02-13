using BlogEngine.Shared.Helpers;

namespace BlogEngine.Shared.DTOs.Blog
{
    public class PostSearchDTO
    {
        public string Title { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Descending;
        public BlogOrderBy BlogOrderBy { get; set; } = BlogOrderBy.DateCreated;
    }
}