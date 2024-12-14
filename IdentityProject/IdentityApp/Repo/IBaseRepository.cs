using System.Linq.Expressions;

namespace IdentityApp.Repo;

public interface IBaseRepository<T>
{
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(Expression<Func<T, bool>> expression);
    T? FindById(int id);
    bool Create(T entity);
    bool Update(T entity);
    bool Delete(T entity);
}
