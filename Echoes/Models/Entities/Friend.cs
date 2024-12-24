﻿namespace Echoes.Models.Entities
{
    public class Friend
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public User FriendUser { get; set; }
        public int FriendId { get; set; }
    }
}
