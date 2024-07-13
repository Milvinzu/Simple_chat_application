namespace Simple_chat_application.DTOs
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public Guid UserId { get; set; }
    }
}
