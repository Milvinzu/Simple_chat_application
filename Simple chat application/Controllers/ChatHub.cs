using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Simple_chat_application.Data;
using Simple_chat_application.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_chat_application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(Guid userId, string message, Guid chatId)
        {
            // Get the user from the database
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userEntity == null)
            {
                throw new Exception("User not found.");
            }

            var userChat = await _context.UserChats.FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);
            if (userChat == null)
            {
                throw new Exception("User is not a member of the chat.");
            }

            var messageEntity = new Message
            {
                MessageId = Guid.NewGuid(),
                Text = message,
                dateTime = DateTime.UtcNow,
                UserId = userEntity.UserId,
                ChatId = chatId
            };

            _context.Messages.Add(messageEntity);
            await _context.SaveChangesAsync();

            var shortTime = messageEntity.dateTime.ToString("HH:mm");

            await Clients.All.SendAsync("ReceiveMessage", userEntity.UserName, message, shortTime);

        }

    }
}
