using Simple_chat_application.Services.Implementations;
using Simple_chat_application.Data;
using Simple_chat_application.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

public class MessageServiceTests
{
    [Fact]
    public async Task AddMessageAsync_AddsMessageToChat()
    {
        var options = new DbContextOptionsBuilder<ChatDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var context = new ChatDbContext(options);
        var messageService = new MessageService(context);

        var chat = new Chat { ChatId = Guid.NewGuid(), ChatName = "testchat" };
        var user = new User { UserId = Guid.NewGuid(), UserName = "testuser" };
        var userChat = new UserChat { UserChatId = Guid.NewGuid(), ChatId = chat.ChatId, UserId = user.UserId };
        context.Chats.Add(chat);
        context.Users.Add(user);
        context.UserChats.Add(userChat);
        await context.SaveChangesAsync();

        var message = new Message { Text = "testmessage", UserId = user.UserId, ChatId = chat.ChatId };

        var result = await messageService.AddMessageAsync(message);

        Assert.Equal("testmessage", result.Text);
        Assert.Equal(user.UserId, result.UserId);
        Assert.Equal(chat.ChatId, result.ChatId);
        Assert.Single(context.Messages.Where(m => m.ChatId == chat.ChatId));
    }
}
