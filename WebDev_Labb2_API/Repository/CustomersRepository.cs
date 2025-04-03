using Microsoft.EntityFrameworkCore;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public class CustomersRepository : Repository<Customers>, ICustomersRepository
    {
        private readonly CustomerMethods _customerMethods;

        public CustomersRepository(DBContext context) : base(context)
        {
            _customerMethods = new CustomerMethods();
        }

        public override async Task<Customers> AddAsync(Customers customer)
        {
            var hashedCustomer = _customerMethods.CreateCustomer(customer);

            await _dbSet.AddAsync(hashedCustomer);
            await _context.SaveChangesAsync();
            return hashedCustomer;
        }

        public async Task<Customers> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.email == email);
        }

        public async Task<Customers> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.username == username);
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(c => c.email == email);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _dbSet.AnyAsync(c => c.username == username);
        }
        public override async Task<IEnumerable<Customers>> GetAllAsync()
        {
            return await _dbSet.OrderBy(c => c.lastname).ThenBy(c => c.firstname).ToListAsync();
        }

    }
}
