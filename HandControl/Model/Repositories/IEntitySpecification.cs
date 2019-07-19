using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Model.Repositories
{
    public interface IEntitySpecification<TEntity>
    {
        bool Specified(TEntity entity);
    }
}
