using Simple_chat_application.DTOs;
using Simple_chat_application.Model;

namespace Simple_chat_application.Services.Interfaces
{
    public interface IMessageService
    {
        Task<Message> AddMessageAsync(Message message);
        Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid chatId, Guid userId);
        Task<bool> ValidateUserInChat(Guid userId, Guid chatId);
    }
}
