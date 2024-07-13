using Microsoft.EntityFrameworkCore;
using Simple_chat_application.Data;
using Simple_chat_application.Model;
using Simple_chat_application.Services.Interfaces;

namespace Simple_chat_application.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _context;

        public ChatService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            var existingChat = await _context.Chats.FirstOrDefaultAsync(u => u.ChatName == chat.ChatName);
            if (existingChat != null)
            {
                throw new Exception("Chat with this name already exists");
            }

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return chat;
        }

        public async Task<IEnumerable<Chat>> GetChatsAsync(string chatName)
        {
            if (string.IsNullOrEmpty(chatName))
            {
                return await _context.Chats.ToListAsync();
            }

            return await _context.Chats
                .Where(c => c.ChatName.Contains(chatName))
                .ToListAsync();
        }

        public async Task DeleteChatAsync(Guid id, Guid userId)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                throw new Exception("Chat not found");
            }

            if (chat.CreatedByUserId != userId)
            {
                throw new Exception("This user is not the owner of the chat");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToChatAsync(Guid userId, Guid chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                throw new Exception("Chat not found");
            }

            var userChat = new UserChat
            {
                UserChatId = Guid.NewGuid(),
                ChatId = chatId,
                UserId = userId
            };

            _context.UserChats.Add(userChat);
            await _context.SaveChangesAsync();
        }
    }
}
