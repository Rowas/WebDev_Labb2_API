using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;
using WebDev_Labb2_API.Repository;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost(Name = "LoginCustomer")]
        public async Task<IActionResult> Post(Login credentials)
        {
            try
            {
                var (customer, verificationResult) = await _loginRepository.ValidateLoginAsync(credentials);

                if (customer == null)
                {
                    return BadRequest(new { message = "Kunden hittades inte eller lösenordet var felaktigt" });
                }

                if (verificationResult != PasswordVerificationResult.Success)
                {
                    return BadRequest(new { message = "Kunden hittades inte eller lösenordet var felaktigt" });
                }

                var token = await _loginRepository.GenerateTokenAsync(customer);

                return Ok(new { message = "Success", token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }
    }
}
