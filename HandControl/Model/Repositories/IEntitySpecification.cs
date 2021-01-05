namespace HandControl.Model.Repositories
{
    public interface IEntitySpecification<TEntity>
    {
        bool Specified(TEntity entity);
    }
}