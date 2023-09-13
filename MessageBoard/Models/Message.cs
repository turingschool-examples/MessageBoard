namespace MessageBoard.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime ExpirationDate { get; set; }
        public User Author { get; set; }

        public bool IsActive()
        {
            return ExpirationDate > DateTime.UtcNow;
        }
    }
}