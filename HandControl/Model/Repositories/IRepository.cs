using System.Collections.Generic;

namespace HandControl.Model.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> Query(IEntitySpecification<TEntity> specification);
    }
}