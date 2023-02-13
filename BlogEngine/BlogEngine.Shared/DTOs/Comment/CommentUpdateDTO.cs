﻿using System.ComponentModel.DataAnnotations;

namespace BlogEngine.Shared.DTOs.Comment
{
    public class CommentUpdateDTO
    {
        [Required]
        [StringLength(300, ErrorMessage = "Comment {0} should not be more than 300 Characters")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}