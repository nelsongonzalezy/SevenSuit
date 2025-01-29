using DbService.Entity;
using Microsoft.EntityFrameworkCore;

namespace DbService.Service.Service
{
    public class SeveClieService : ISeveClieInterface
    {
        private readonly RepositoryService<SeveClie> _repository;
        private readonly Context _context;

        public SeveClieService(Context context) {
            _repository = new RepositoryService<SeveClie>(context);
            _context = context;
        }

        public async Task<IEnumerable<SeveClie>> GetAllSeveClie()
        {
            return await _context.Clientes
                .Include(c => c.EstadoCivil) 
                .Select(c => new SeveClie
                {
                    Id = c.Id,
                    Cedula = c.Cedula,
                    Nombre = c.Nombre,
                    Genero = c.Genero,
                    Fecha_nac = c.Fecha_nac,
                    Id_EstadoCivil = c.Id_EstadoCivil,
                    EstadoCivil = c.EstadoCivil 
                })
                .ToListAsync();
        }

        public async Task<bool> CreateSeveClie(SeveClie model) 
        {
            return await _repository.Create(model);
        }
        public async Task<bool> UpdateSeveClie(SeveClie model)
        {
            return await _repository.Update(model);
        }       
        public async Task<SeveClie> GetSeveClieById(Guid Key)
        {
            return await _repository.GetByKey(Key);
        }
        public async Task<bool> HardDelete(Guid Key)
        {
            var result = await _repository.HardDelete(Key);
            return result;
        }

    }
}
