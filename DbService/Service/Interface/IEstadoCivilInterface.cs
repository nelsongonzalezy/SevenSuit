using DbService.Entity;

namespace DbService
{
    public interface IEstadoCivilInterface
    {
        Task<IEnumerable<SeveEstadoCivil>> GetAllSeveClie();
    }
}
