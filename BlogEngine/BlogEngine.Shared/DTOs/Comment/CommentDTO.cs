using BlogEngine.Shared.DTOs.Common;
using BlogEngine.Shared.DTOs.Identity;

namespace BlogEngine.Shared.DTOs.Comment
{
    public class CommentDTO : ReadDataDTOBase
    {
        public int ID { get; set; }
        public int PostID { get; set; }
        public int ApplicationUserID { get; set; }
        public UserInfoDetailDTO UserInfoDetailDTO { get; set; }
        public string Body { get; set; }
    }
}