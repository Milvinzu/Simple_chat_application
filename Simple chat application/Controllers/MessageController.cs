using Microsoft.AspNetCore.Mvc;
using Simple_chat_application.DTOs;
using Simple_chat_application.Model;
using Simple_chat_application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple_chat_application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            try
            {
                var addedMessage = await _messageService.AddMessageAsync(message);
                return Ok("Message added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetMessages(Guid chatId, [FromQuery] Guid userId)
        {
            try
            {
                var messages = await _messageService.GetMessagesAsync(chatId, userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
