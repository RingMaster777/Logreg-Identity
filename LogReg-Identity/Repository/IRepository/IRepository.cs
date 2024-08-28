using System.Linq.Expressions;

namespace LogReg_Identity.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T

        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity); 
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
