using Simple_chat_application.Model;

namespace Simple_chat_application.Services.Interfaces
{
    public interface IChatService
    {
        Task<Chat> CreateChatAsync(Chat chat);
        Task<IEnumerable<Chat>> GetChatsAsync(string chatName);
        Task DeleteChatAsync(Guid id, Guid userId);
        Task AddUserToChatAsync(Guid userId, Guid chatId);
    }
}
