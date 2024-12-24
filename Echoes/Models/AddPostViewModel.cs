using Echoes.Models.Entities;

namespace Echoes.Models
{
    public class AddPostViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int UserId { get; set; }
    }
}
