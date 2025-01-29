using DbService.Entity;
using Microsoft.EntityFrameworkCore;

namespace DbService.Service.Service
{
    public class UserService : IUserInterface
    {
        private readonly Context _context;

        public UserService(Context context) { _context = context; }

        public async Task<Users> Authenticate(string mail, string password)
        {
            var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.Mail == mail.Trim().ToLower());

            if (user == null || user.Password != password)
            {
                return null;
            }
            return user;
        }

    }
}
