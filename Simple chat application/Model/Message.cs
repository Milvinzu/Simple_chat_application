using Microsoft.VisualBasic;

namespace Simple_chat_application.Model
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string Text { get; set; }
        public DateTime dateTime { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid ChatId { get; set; }
        public Chat? Chat { get; set; }
    }
}
