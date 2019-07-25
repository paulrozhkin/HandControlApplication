using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Model.Repositories.GestureRepositories.Specifications
{
    public class GesturesSpecificationById : IEntitySpecification<GestureModel>
    {
        private readonly Guid expectedIdField;

        public GesturesSpecificationById(Guid expectedId)
        {
            this.expectedIdField = expectedId;
        }

        public bool Specified(GestureModel entity)
        {
            return this.expectedIdField.Equals(entity.Id);
        }
    }
}
