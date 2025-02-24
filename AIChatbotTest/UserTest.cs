using AIChatbot.API;
using AIChatbot.Business;
using AIChatbot.Data;
using AIChatbot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Testcontainers.PostgreSql;

namespace AIChatbotTest;

[TestFixture]
public class UserTest
{
    private PostgreSqlContainer _postgresContainer;
    private ChatbotDbContext _dbContext;
    private IUserBusiness _userBusiness;

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
    public void WhenInsertingUser_ThenReturnTrue()
    {
        User user = new()
        {
            Id = 512,
            Username = "az",
            Password = "azOtn0v0"
        };

        _userBusiness.Add(user);

        User? actualUser = _userBusiness.GetAll().FirstOrDefault(u => u.Username == user.Username);

        Assert.That(_userBusiness.Find(user), Is.True);
        Assert.That(actualUser.Id, Is.EqualTo(user.Id));
        Assert.That(actualUser.Username, Is.EqualTo(user.Username));
        Assert.That(actualUser.Password, Is.EqualTo(user.Password));
    }

    [Test]
    public void WhenGettingAllUsers_ThenReturnTrue()
    {
        User user1 = new()
        {
            Id = 512,
            Username = "az",
            Password = "nz"
        };

        User user2 = new()
        {
            Id = 312,
            Username = "ti",
            Password = "da"
        };

        User user3 = new()
        {
            Id = 112,
            Username = "tq",
            Password = "azOazzzzzzztn0v0"
        };

        _userBusiness.Add(user1);
        _userBusiness.Add(user2);
        _userBusiness.Add(user3);

        List<User> users = _userBusiness.GetAll();

        Assert.That(users, Has.Count.EqualTo(3));
        Assert.That(users.Any(u => u.Username == "az"), Is.True);
        Assert.That(users.Any(u => u.Username == "ti"), Is.True);
        Assert.That(users.Any(u => u.Username == "tq"), Is.True);
    }

    [Test]
    public void WhenFindingExistingUser_ThenReturnTrue()
    {
        User user = new() 
        { 
            Id = 3, 
            Username = "ikona", 
            Password = "secure123" 
        };

        _userBusiness.Add(user);

        bool exists = _userBusiness.Find(user);

        Assert.That(exists, Is.True);
    }

    [Test]
    public void WhenFindingNonexistingUser_ThenReturnFalse()
    {
        User user = new()
        {
            Id = 3,
            Username = "ikona",
            Password = "secure123"
        };

        User user1 = new()
        {
            Id = 34,
            Username = "Charlie ot wish",
            Password = "secure123321"
        };

        _userBusiness.Add(user);

        bool exists = _userBusiness.Find(user1);

        Assert.That(exists, Is.False);
    }

    [Test]
    public void WhenFindingExistentUserByUsername_ThenReturnTrue()
    {
        User user = new()
        {
            Id = 3,
            Username = "ikona",
            Password = "secure123"
        };

        _userBusiness.Add(user);

        bool exists = _userBusiness.FindByUsername("ikona");

        Assert.That(exists, Is.True);
    }

    [Test]
    public void WhenFindingNonexistentUserByUsername_ThenReturnFalse()
    {
        bool exists = _userBusiness.FindByUsername("Charlie XCX");

        Assert.That(exists, Is.False);
    }

    [Test]
    public void WhenAddingUserWithUsernameMoreThan50Characters_ThenThrowsException()
    {
        User user = new()
        {
            Id = 5,
            Username = new string('a', 100),
            Password = "12345"
        };

        Assert.Throws<DbUpdateException>(() => _userBusiness.Add(user));
    }

    [Test]
    public void WhenFindingUsernameWithDifferentCase_ThenReturnFalse()
    {
        User user = new()
        {
            Id = 5,
            Username = "poli",
            Password = "12345"
        };

        _userBusiness.Add(user);

        bool exists = _userBusiness.FindByUsername("PoLI");

        Assert.That(exists, Is.False);
    }
}
