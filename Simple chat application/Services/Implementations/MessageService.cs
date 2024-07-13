using Microsoft.EntityFrameworkCore;
using Simple_chat_application.Data;
using Simple_chat_application.DTOs;
using Simple_chat_application.Model;
using Simple_chat_application.Services.Interfaces;

namespace Simple_chat_application.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly ChatDbContext _context;

        public MessageService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            var userChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == message.ChatId && uc.UserId == message.UserId);
            if (userChat == null)
            {
                throw new Exception("User is not a member of the chat.");
            }

            message.dateTime = DateTime.UtcNow;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesAsync(Guid chatId, Guid userId)
        {
            var userChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);
            if (userChat == null)
            {
                throw new Exception("User is not a member of the chat.");
            }

            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .Select(m => new MessageDto
                {
                    MessageId = m.MessageId,
                    Text = m.Text,
                    DateTime = m.dateTime,
                    UserId = m.UserId
                })
                .ToListAsync();
        }

        public async Task<bool> ValidateUserInChat(Guid userId, Guid chatId)
        {
            return await _context.UserChats.AnyAsync(uc => uc.ChatId == chatId && uc.UserId == userId);
        }
    }
}
