namespace Simple_chat_application.Model
{
    public class UserChat
    {
        public Guid UserChatId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid ChatId { get; set; }
        public Chat? Chat { get; set; }
    }
}
