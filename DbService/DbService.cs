using Microsoft.Extensions.Logging;

namespace DbService
{
    public class DbServiceContext
    {
        private readonly ILogger _logger;
        private readonly Context _context;

        public DbServiceContext(Context context ,ILogger<DbServiceContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Initialize()
        {
            _context.Seeder(_logger);
        }
    }
}
