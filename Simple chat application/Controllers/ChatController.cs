using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple_chat_application.Model;
using Simple_chat_application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_chat_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            try
            {
                var createdChat = await _chatService.CreateChatAsync(chat);
                return CreatedAtAction(nameof(PostChat), new { id = createdChat.ChatId }, createdChat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats([FromQuery] string chatName = null)
        {
            var chats = await _chatService.GetChatsAsync(chatName);
            return Ok(chats);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(Guid id, [FromHeader(Name = "UserId")] Guid userId)
        {
            try
            {
                await _chatService.DeleteChatAsync(id, userId);
                return Ok("Chat was deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddUserToChat")]
        public async Task<IActionResult> AddUserToChat([FromQuery] Guid userId, [FromQuery] Guid chatId)
        {
            try
            {
                await _chatService.AddUserToChatAsync(userId, chatId);
                return Ok("User added to chat successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
