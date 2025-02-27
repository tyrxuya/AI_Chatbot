using AIChatbot.API;
using AIChatbot.Business;
using AIChatbot.Data;
using AIChatbot.Data.Models;
using Microsoft.EntityFrameworkCore;
using OpenAI.Chat;
using Testcontainers.PostgreSql;

namespace AIChatbotTest;

[TestFixture]
public class ChatroomTest
{
    private PostgreSqlContainer _postgresContainer;
    private ChatbotDbContext _dbContext;

    private UserBusiness _userBusiness;
    private ChatroomBusiness _chatroomBusiness;

    private User _testUser;
    private Chatroom _testChatroom;

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
    public void WhenInsertingChatroom_ThenReturnTrue() 
    {
        User? actualUser = _userBusiness.GetAll().FirstOrDefault(u => u.Username == _testUser.Username);
        Chatroom? actualChatroom = _chatroomBusiness.GetAll().FirstOrDefault(ch => ch.Title == _testChatroom.Title && ch.UserId == _testUser.Id);

        Assert.That(_userBusiness.Find(_testUser), Is.Not.Null);
        Assert.That(_chatroomBusiness.Find(_testChatroom), Is.Not.Null);

        Assert.That(_testChatroom.Id, Is.EqualTo(actualChatroom.Id));
        Assert.That(_testChatroom.Title, Is.EqualTo(actualChatroom.Title));
        Assert.That(_testChatroom.User, Is.EqualTo(actualUser));
    }

    [Test]
    public void WhenGettingAllChatrooms_ThenReturnTrue() 
    {
        Chatroom chatroom1 = new()
        {
            Title = "Test chatroom1",
            UserId = _testUser.Id
        };

        Chatroom chatroom2 = new()
        {
            Title = "Test chatroom2",
            UserId = _testUser.Id
        };

        _chatroomBusiness.Add(chatroom1);
        _chatroomBusiness.Add(chatroom2);

        User? actualUser = _userBusiness.GetAll().FirstOrDefault(u => u.Username == _testUser.Username);
        List<Chatroom> allChatrooms = _chatroomBusiness.GetAll();

        Assert.That(_userBusiness.Find(_testUser), Is.Not.Null);
        Assert.That(_chatroomBusiness.GetAll(), Is.Not.Empty);

        Assert.That(_chatroomBusiness.GetAll(), Has.Count.EqualTo(3));
        Assert.That(allChatrooms.FirstOrDefault(ch => ch.Title == _testChatroom.Title), Is.Not.Null);
        Assert.That(allChatrooms.FirstOrDefault(ch => ch.Title == chatroom1.Title), Is.Not.Null);
        Assert.That(allChatrooms.FirstOrDefault(ch => ch.Title == chatroom2.Title), Is.Not.Null);
    }

    [Test]
    public void WhenFindingExistingChatroom_ThenReturnTrue() 
    {
        User? actualUser = _userBusiness.GetAll().FirstOrDefault(u => u.Username == _testUser.Username);
        Chatroom? actualChatroom = _chatroomBusiness.FindByUserAndTitle(_testUser, _testChatroom.Title);

        Assert.That(actualChatroom, Is.Not.Null);
        Assert.That(actualChatroom.Id, Is.EqualTo(_testChatroom.Id));
        Assert.That(actualChatroom.Title, Is.EqualTo(_testChatroom.Title));
        Assert.That(actualChatroom.UserId, Is.EqualTo(_testUser.Id));
    }

    [Test]
    public void WhenFindingNonExistingChatroomByUser_ThenReturnFalse() 
    {
        User? actualUser = _userBusiness.GetAll().FirstOrDefault(u => u.Username == _testUser.Username);
        Chatroom? actualChatroom = _chatroomBusiness.FindByUserAndTitle(_testUser, "abcd");

        Assert.That(actualChatroom, Is.Null);
    }

    [Test]
    public void WhenFindingNonExistingChatroomByUserAndTitle_ThenReturnFalse() 
    {
        User? actualUser = _userBusiness.GetAll().FirstOrDefault(u => u.Username == _testUser.Username);
        Chatroom? actualChatroom = _chatroomBusiness.FindByUserAndTitle(_testUser, "abcd");

        Assert.That(actualChatroom, Is.Null);
    }

    [Test]
    public void WhenAddingChatroomWithLongTitle_ThenThrowException() 
    {
        Chatroom chatroom = new()
        {
            Title = new('a', 500),
            UserId = _testUser.Id
        };

        Assert.Throws<DbUpdateException>(() => _chatroomBusiness.Add(chatroom));
    }
}
