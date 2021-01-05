namespace HandControl.Model.Repositories.GestureRepositories.Specifications
{
    public class GesturesSpecificationByAll : IEntitySpecification<GestureModel>
    {
        public bool Specified(GestureModel entity)
        {
            return true;
        }
    }
}