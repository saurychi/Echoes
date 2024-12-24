namespace Echoes.Models
{
    public class CommentViewModel
    {
        public string CommentContent { get; set; }
        public int PostId { get; set; }
        public int CommenterId { get; set; }
        public string CommenterName { get; set; }
    }
}
