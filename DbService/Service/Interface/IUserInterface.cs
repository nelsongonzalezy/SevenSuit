using DbService.Entity;

namespace DbService
{
    public interface IUserInterface
    {
        Task<Users> Authenticate(string mail, string password);
    }
}
