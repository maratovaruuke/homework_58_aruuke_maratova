namespace homework_59_aruuke_maratova.Models
{
    public class Like
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
