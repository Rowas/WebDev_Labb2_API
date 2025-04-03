using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly DBContext _context;
        private readonly CustomerMethods _customerMethods;
        private readonly GetJWT _getJWT;

        public LoginRepository(DBContext context)
        {
            _context = context;
            _customerMethods = new CustomerMethods();
            _getJWT = new GetJWT();
        }

        public async Task<(Customers customer, PasswordVerificationResult result)> ValidateLoginAsync(Login credentials)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.email == credentials.email);

            if (customer == null)
            {
                return (null, PasswordVerificationResult.Failed);
            }

            var verificationResult = _customerMethods.VerifyLogin(customer, credentials.password);
            return (customer, verificationResult);
        }

        public async Task<string> GenerateTokenAsync(Customers customer)
        {
            return _getJWT.GenerateJwtToken(customer.username, customer.userlevel);
        }
    }
} 