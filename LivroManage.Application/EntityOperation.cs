using System.Collections.Generic;
using System.Threading.Tasks;

namespace LivroManage.Application
{
    public abstract class EntityOperation<T>
    {
        public abstract Task Create(T model);
        public abstract Task Update(T model);
        public abstract Task Delete(int id);
        public abstract T Get(int? id);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> GetAll(int canal);
    }
}
