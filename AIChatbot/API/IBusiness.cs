namespace AIChatbot.API
{
    public interface IBusiness<T>
    {
        public List<T> GetAll();
        public void Add(T t);
        public T? Find(T t);
        public void Remove(int id);
    }
}
