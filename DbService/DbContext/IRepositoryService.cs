namespace DbService
{
    public interface IRepositoryService<T> where T : class
    {
        Task<T> GetByKey(Guid id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> Update(T entidad);
        Task<bool> Create(T entidad);
        Task<bool> HardDelete(Guid id);
    }
}
