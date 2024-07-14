using Simple_chat_application.Services.Implementations;
using Simple_chat_application.Data;
using Simple_chat_application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public async Task CreateUserAsync_ThrowsException_WhenUserExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        using (var context = new ChatDbContext(options))
        {
            var userService = new UserService(context);

            var user = new User { UserName = "testuser" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            await Assert.ThrowsAsync<Exception>(() => userService.CreateUserAsync(new User { UserName = "testuser" }));
        }
    }
}