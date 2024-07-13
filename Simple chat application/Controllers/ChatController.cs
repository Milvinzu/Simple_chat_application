using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_chat_application.Data;
using Simple_chat_application.Model;

namespace Simple_chat_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatDbContext _context;

        public ChatController(ChatDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            var existingChat = await _context.Chats.FirstOrDefaultAsync(u => u.ChatName == chat.ChatName);

            if (existingChat != null)
            {
                return BadRequest("Chat with this name already exists");
            }

            var createdByUser = await _context.Users.FindAsync(chat.CreatedByUserId);
            if (createdByUser == null)
            {
                return BadRequest("Invalid user ID");
            }

            chat.CreatedByUser = createdByUser;

            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            var userChat = new UserChat
            {
                UserChatId = Guid.NewGuid(),
                ChatId = chat.ChatId,
                UserId = chat.CreatedByUserId
            };

            _context.UserChats.Add(userChat);
            await _context.SaveChangesAsync();

            return Ok("Chat successfully created");
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats([FromQuery] string chatName = null)
        {
            if (string.IsNullOrEmpty(chatName))
            {
                return await _context.Chats.ToListAsync();
            }

            var chats = await _context.Chats
                .Where(c => c.ChatName.Contains(chatName))
                .ToListAsync();

            return Ok(chats);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(Guid id, [FromHeader(Name = "UserId")] Guid userId)
        {

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            if (chat.CreatedByUserId != userId)
            {
                return Forbid("This user not owner");
            }

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();

            return Ok("Chat was deleted");
        }

        [HttpPost("AddUserToChat")]
        public async Task<IActionResult> AddUserToChat([FromQuery] Guid userId, [FromQuery] Guid chatId)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                return NotFound("Chat not found.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userChat = new UserChat
            {
                UserChatId = Guid.NewGuid(),
                ChatId = chatId,
                UserId = userId
            };

            _context.UserChats.Add(userChat);
            await _context.SaveChangesAsync();

            return Ok("User added to chat successfully.");
        }
    }
}
