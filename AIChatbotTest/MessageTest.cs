using AIChatbot.Business;
using AIChatbot.Data;
using AIChatbot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace AIChatbotTest;

[TestFixture]
public class MessageTest
{
    private PostgreSqlContainer _postgresContainer;
    private ChatbotDbContext _dbContext;

    private UserBusiness _userBusiness;
    private ChatroomBusiness _chatroomBusiness;
    private MessageBusiness _messageBusiness;

    private User _testUser;
    private Chatroom _testChatroom;
    private Message _testMessage;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("test_db")
            .WithUsername("test")
            .WithPassword("test")
            .WithImage("postgres:latest")
            .WithCleanUp(true)
            .Build();

        await _postgresContainer.StartAsync();
    }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ChatbotDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString())
        .Options;

        _dbContext = new ChatbotDbContext(options);
        _dbContext.Database.EnsureCreated();

        _userBusiness = new UserBusiness(_dbContext);
        _chatroomBusiness = new ChatroomBusiness(_dbContext);
        _messageBusiness = new MessageBusiness(_dbContext);

        _testUser = new()
        {
            Username = "testUser",
            Password = "123"
        };

        _userBusiness.Add(_testUser);

        _testChatroom = new()
        {
            Title = "Test chatroom",
            UserId = _testUser.Id
        };

        _chatroomBusiness.Add(_testChatroom);

        _testMessage = new()
        {
            Content = "Hello, what a great day it is today!",
            Type = APIConstants.USER_MESSAGE,
            ChatroomId = _testChatroom.Id
        };

        _messageBusiness.Add(_testMessage);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [OneTimeTearDown]
    public async Task GlobalCleanup()
    {
        await _postgresContainer.DisposeAsync();
    }

    [Test]
    public void WhenInsertingMessage_ThenReturnTrue() 
    {
        Message? actualMessage = _messageBusiness.GetAll().First();

        Assert.That(actualMessage, Is.Not.Null);
        Assert.That(_testMessage.Id, Is.EqualTo(actualMessage.Id));
        Assert.That(_testMessage.Content, Is.EqualTo(actualMessage.Content));
        Assert.That(_testMessage.ChatroomId, Is.EqualTo(actualMessage.ChatroomId));
    }

    [Test]
    public void WhenGettingAllMessagesInChatroom_ThenReturnTrue() 
    { 
        Message message1 = new()
        {
            Content = "Hello, what a GREAT day it is today!",
            Type = APIConstants.AI_MESSAGE,
            ChatroomId = _testChatroom.Id
        };

        Message message2 = new()
        {
            Content = "Hello, what a bad day it is today.",
            Type = APIConstants.USER_MESSAGE,
            ChatroomId = _testChatroom.Id
        };

        _messageBusiness.Add(message1);
        _messageBusiness.Add(message2);

        List<Message> actualMessages = _messageBusiness.GetByChatroom(_testChatroom);

        Assert.That(actualMessages, Has.Count.EqualTo(3));
        Assert.That(actualMessages.Any(m => m.Content == _testMessage.Content), Is.True);
        Assert.That(actualMessages.Any(m => m.Content == message1.Content), Is.True);
        Assert.That(actualMessages.Any(m => m.Content == message2.Content), Is.True);
    }

    [Test]
    public void WhenAddingMessageWithLongContent_ThenThrowException() 
    {
        Message longMessage = new()
        {
            Content = new string('a', 500),
            Type = APIConstants.AI_MESSAGE,
            ChatroomId = _testChatroom.Id
        };

        Assert.Throws<DbUpdateException>(() => _messageBusiness.Add(longMessage));
    }

    [Test]
    public void WhenAddingMessageWithoutChatroom_ThenThrowException() 
    {
        Message longMessage = new()
        {
            Content = _testMessage.Content,
            Type = APIConstants.AI_MESSAGE
        };

        Assert.Throws<DbUpdateException>(() => _messageBusiness.Add(longMessage));
    }
}
