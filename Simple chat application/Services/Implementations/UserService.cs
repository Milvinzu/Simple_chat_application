using Microsoft.EntityFrameworkCore;
using Simple_chat_application.Data;
using Simple_chat_application.Model;
using Simple_chat_application.Services.Interfaces;

namespace Simple_chat_application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ChatDbContext _context;

        public UserService(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (existingUser != null)
            {
                throw new Exception("Nickname already exists");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }
    }
}
