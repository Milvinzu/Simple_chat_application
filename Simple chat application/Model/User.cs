namespace Simple_chat_application.Model
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<UserChat> UserChats { get; set; } = new List<UserChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
