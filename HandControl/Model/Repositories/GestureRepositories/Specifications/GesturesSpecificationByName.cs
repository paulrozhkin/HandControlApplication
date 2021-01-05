namespace HandControl.Model.Repositories.GestureRepositories.Specifications
{
    public class GesturesSpecificationByName : IEntitySpecification<GestureModel>
    {
        private readonly string expectedNameField;

        public GesturesSpecificationByName(string expectedName)
        {
            expectedNameField = expectedName;
        }

        public bool Specified(GestureModel entity)
        {
            return expectedNameField.Equals(entity.Name);
        }
    }
}