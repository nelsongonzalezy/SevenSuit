using DbService.Entity;

namespace DbService.Service.Service
{
    public  class EstadoCivilService : IEstadoCivilInterface
    {
        private readonly RepositoryService<SeveEstadoCivil> _repository;

        public EstadoCivilService(Context context)
        {
            _repository = new RepositoryService<SeveEstadoCivil>(context);
        }
        public async Task<IEnumerable<SeveEstadoCivil>> GetAllSeveClie()
        {
            return await _repository.GetAllAsync();
        }

    }
}
