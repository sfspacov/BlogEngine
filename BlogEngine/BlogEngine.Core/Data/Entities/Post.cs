using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogEngine.Core.Data.Entities.Common;

namespace BlogEngine.Core.Data.Entities
{
    public class Post : BaseEntity
    {
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserID { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should not be more than 50 Characters")]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        [DataType(DataType.Html)]
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        public string EditorsReview { get; set; }
        public int EstimatedReadingTimeInMinutes { get; set; }
        public DateTime Published { get; set; }
        public int PostStatusID { get; set; }
        public PostStatus PostStatus { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}