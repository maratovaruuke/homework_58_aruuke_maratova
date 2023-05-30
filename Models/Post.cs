namespace homework_59_aruuke_maratova.Models
{
    public class Post
    {
        public int Id { get; set; }
        public byte[] Img { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public Post()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
        }
    }
}
