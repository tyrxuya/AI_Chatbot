using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIChatbot.Data.Models;

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
