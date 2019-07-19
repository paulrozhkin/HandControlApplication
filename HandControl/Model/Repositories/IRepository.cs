using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Model.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    { 
        void Add(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> Query(IEntitySpecification<TEntity> specification);
    }
}
