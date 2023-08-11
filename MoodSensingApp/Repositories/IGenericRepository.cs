using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoodSensingApp.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        IEnumerable<TEntity> GetAll();
        
        TEntity GetById(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null, string includeProperties = "");


    }
}