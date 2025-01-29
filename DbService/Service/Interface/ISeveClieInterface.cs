using DbService.Entity;

namespace DbService
{
    public interface ISeveClieInterface
    {
        Task<IEnumerable<SeveClie>> GetAllSeveClie();
        Task<bool> CreateSeveClie(SeveClie model);
        Task<bool> UpdateSeveClie(SeveClie model);
        Task<SeveClie> GetSeveClieById(Guid Key);
        Task<bool> HardDelete(Guid Key);
    }
}
