using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_chat_application.Data;
using Simple_chat_application.DTOs;
using Simple_chat_application.Model;

namespace Simple_chat_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ChatDbContext _context;

        public MessageController(ChatDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            var userChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == message.ChatId && uc.UserId == message.UserId);

            if (userChat == null)
            {
                return BadRequest("User is not a member of the chat.");
            }

            var chat = await _context.Chats.FindAsync(message.ChatId);
            if (chat == null)
            {
                return NotFound("Chat not found.");
            }

            var user = await _context.Users.FindAsync(message.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            message.dateTime = DateTime.UtcNow;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok("Message added successfully.");
        }



        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetMessages(Guid chatId, [FromQuery] Guid userId)
        {
            var userChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);

            if (userChat == null)
            {
                return BadRequest("User is not a member of the chat.");
            }

            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                return NotFound("Chat not found.");
            }

            var messages = await _context.Messages
                .Where(m => m.ChatId == chatId)
                .Select(m => new MessageDto
                {
                    MessageId = m.MessageId,
                    Text = m.Text,
                    DateTime = m.dateTime,
                    UserId = m.UserId
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}
