using Microsoft.AspNetCore.Identity;
using WebDev_Labb2_API.Model;

namespace WebDev_Labb2_API.Repository
{
    public interface ILoginRepository
    {
        Task<(Customers customer, PasswordVerificationResult result)> ValidateLoginAsync(Login credentials);
        Task<string> GenerateTokenAsync(Customers customer);
    }
} 