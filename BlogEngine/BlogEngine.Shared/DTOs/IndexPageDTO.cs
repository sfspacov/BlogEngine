using BlogEngine.Shared.DTOs.Blog;
using System.Collections.Generic;

namespace BlogEngine.Shared.DTOs
{
    public class IndexPageDTO
    {
        public List<PostDTO> NewPostDTOs { get; set; } = new List<PostDTO>();
    }
}