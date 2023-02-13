using BlogEngine.Shared.DTOs.Blog;
using System.Collections.Generic;

namespace BlogEngine.Shared.DTOs.Identity
{
    public class UserProfileDTO : UserInfoDetailDTO
    {
        public List<PostDTO> PostDTOs { get; set; } = new List<PostDTO>();
    }
}