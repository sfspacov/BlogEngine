using System.ComponentModel.DataAnnotations;

namespace BlogEngine.Shared.DTOs.Blog
{
    public class PostCreationDTO
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should not be more than 100 Characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
    }
}