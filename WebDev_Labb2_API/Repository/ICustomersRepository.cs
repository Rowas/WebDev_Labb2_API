using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public interface ICustomersRepository : IRepository<Customers>
    {
        Task<Customers> GetByEmailAsync(string email);
        Task<Customers> GetByUsernameAsync(string username);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
    }
}
