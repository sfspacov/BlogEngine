using BlogEngine.Core.Data.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogEngine.Core.Data.Entities
{
    public class PostRating : BaseEntity
    {
        public int Rate { get; set; }
        public int PostID { get; set; }
        public Post Blog { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserID { get; set; }
    }
}