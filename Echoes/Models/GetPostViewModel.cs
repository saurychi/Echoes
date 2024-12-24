namespace Echoes.Models
{
    public class GetPostViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public int UserId { get; set; }
        public string Author { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
