using System;

namespace HandControl.Model.Repositories.GestureRepositories.Specifications
{
    public class GesturesSpecificationById : IEntitySpecification<GestureModel>
    {
        private readonly Guid expectedIdField;

        public GesturesSpecificationById(Guid expectedId)
        {
            expectedIdField = expectedId;
        }

        public bool Specified(GestureModel entity)
        {
            return expectedIdField.Equals(entity.Id);
        }
    }
}