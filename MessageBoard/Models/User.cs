namespace MessageBoard.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordDigest { get; set; }
        public List<Message> Messages { get; set; }
        public Role Role { get; set; }
    }
}
