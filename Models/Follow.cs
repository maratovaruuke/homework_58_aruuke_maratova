namespace homework_59_aruuke_maratova.Models
{
    public class Follow
    {
        public string Id { get; set; }

        public string FollowerId { get; set; }
        public User Follower { get; set; }

        public string FolloweeId { get; set; }
        public User Followee { get; set; }

        public DateTime CreatedAt { get; set; }

        public Follow()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
