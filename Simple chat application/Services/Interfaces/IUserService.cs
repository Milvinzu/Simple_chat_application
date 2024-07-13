using Simple_chat_application.Model;

namespace Simple_chat_application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserAsync(Guid id);
    }
}
