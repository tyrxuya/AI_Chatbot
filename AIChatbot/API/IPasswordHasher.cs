namespace AIChatbot.API
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
    }
}
