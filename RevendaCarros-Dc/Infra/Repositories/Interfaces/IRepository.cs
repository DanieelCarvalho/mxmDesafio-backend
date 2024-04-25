namespace RevendaCarros_Dc.Infra.Repositories.Interfaces;

public interface IRepository<T>
{
    Task<IEnumerable<T>> FindAll();

    Task<T?> FindById(int id);

    Task Save(T entity);

    Task Update( T entity);

    Task<bool> DeleteById(int id);

}
