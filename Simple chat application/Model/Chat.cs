namespace Simple_chat_application.Model
{
    public class Chat
    {
        public Guid ChatId { get; set; }
        public string ChatName { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public ICollection<UserChat> UserChats { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
