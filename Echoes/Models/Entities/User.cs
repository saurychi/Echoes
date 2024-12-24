namespace Echoes.Models.Entities
{
    public enum ActiveStatus
    {
        Active,
        Inactive,
        Banned
    }
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ActiveStatus ActiveStatus { get; set; }

        // navigation properties
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Friend> Accounts { get; set; }
        public ICollection<Friend> Friends { get; set; }


    }
}
