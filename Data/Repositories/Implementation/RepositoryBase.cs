using ProjectLab.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProjectLab.Data.Repositories.Implementation
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDBContext applicationDBContext { get; set; }

        public RepositoryBase(ApplicationDBContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.applicationDBContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.applicationDBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.applicationDBContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.applicationDBContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.applicationDBContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            this.applicationDBContext.SaveChanges();
        }
    }
}
