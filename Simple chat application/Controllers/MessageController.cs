using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple_chat_application.Data;

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
    }
}
