using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdentityApp.Repo;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly IdentityContext _context;

    public BaseRepository(IdentityContext ctx)
    {
        _context = ctx;
    }

    public bool Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        _context.SaveChanges();
        return true;
    }
    public bool Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
        return true;
    }
    public T? FindById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking();
    }

    public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression).AsNoTracking();
    }
}
