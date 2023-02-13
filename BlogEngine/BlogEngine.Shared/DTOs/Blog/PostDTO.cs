using BlogEngine.Shared.DTOs.Comment;
using BlogEngine.Shared.DTOs.Common;
using System.Collections.Generic;

namespace BlogEngine.Shared.DTOs.Blog
{
    public class PostDTO : ReadDataDTOBase
    {
        public int ID { get; set; }
        public int ApplicationUserID { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string EditorsReview { get; set; }
        public string Status { get; set; }
        public int EstimatedReadingTimeInMinutes { get; set; }

        public List<CommentDTO> CommentDTOs { get; set; } = new List<CommentDTO>();
    }
}