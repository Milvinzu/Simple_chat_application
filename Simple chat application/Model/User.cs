namespace Simple_chat_application.Model
{
    public class User
    {
        public Guid UserId { get; set; }
        public ICollection<UserChat> UserChats { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
