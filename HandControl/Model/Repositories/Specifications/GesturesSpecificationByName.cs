using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Model.Repositories.Specifications
{
    public class GesturesSpecificationByName : IEntitySpecification<GestureModel>
    {
        private readonly string expectedNameField;

        public GesturesSpecificationByName(string expectedName)
        {
            this.expectedNameField = expectedName;
        }

        public bool Specified(GestureModel entity)
        {
            return this.expectedNameField.Equals(entity.Name);
        }
    }
}
