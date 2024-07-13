using Microsoft.AspNetCore.SignalR;
using Simple_chat_application.Services.Interfaces;
using Simple_chat_application.Model;
using System;
using System.Threading.Tasks;

namespace Simple_chat_application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(Guid userId, string message, Guid chatId)
        {
            var userChat = await _messageService.ValidateUserInChat(userId, chatId);
            if (!userChat)
            {
                throw new HubException("User is not a member of the chat.");
            }

            var messageEntity = new Message
            {
                MessageId = Guid.NewGuid(),
                Text = message,
                dateTime = DateTime.UtcNow,
                UserId = userId,
                ChatId = chatId
            };

            await _messageService.AddMessageAsync(messageEntity);

            var shortTime = messageEntity.dateTime.ToString("HH:mm");
            await Clients.All.SendAsync("ReceiveMessage", userId, message, shortTime);
        }
    }
}
