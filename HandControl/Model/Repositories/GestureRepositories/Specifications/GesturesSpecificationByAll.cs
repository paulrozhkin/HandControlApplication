using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Model.Repositories.GestureRepositories.Specifications
{
    public class GesturesSpecificationByAll : IEntitySpecification<GestureModel>
    { 
        public GesturesSpecificationByAll()
        {

        }

        public bool Specified(GestureModel entity)
        {
            return true;
        }
    }
}
