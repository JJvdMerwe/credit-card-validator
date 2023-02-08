namespace Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T: class
    {
        IAsyncEnumerable<T> GetAll();
        Task<T?> GetByIdAsync(object id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
