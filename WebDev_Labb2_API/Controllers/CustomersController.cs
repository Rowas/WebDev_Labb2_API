using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebDev_Labb2_API.Model;
using WebDev_Labb2_API.Repository;

namespace WebDev_Labb2_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersController(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        [HttpGet(Name = "GetCustomers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Customers>>> Get()
        {
            try
            {
                var customers = await _customersRepository.GetAllAsync();
                return Ok(customers);
            }
            catch
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpGet("{email}", Name = "GetCustomer")]
        [Authorize]
        public async Task<ActionResult<Customers>> Get(string email)
        {
            try
            {
                var customer = await _customersRepository.GetByEmailAsync(email);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPost(Name = "AddCustomer")]
        public async Task<ActionResult<Customers>> Post(Customers receivedCustomer)
        {
            try
            {
                if (await _customersRepository.ExistsByEmailAsync(receivedCustomer.email))
                {
                    return BadRequest(new { message = "Customer already exists." });
                }

                if (await _customersRepository.ExistsByUsernameAsync(receivedCustomer.username))
                {
                    receivedCustomer.username = receivedCustomer.username + "1";
                }

                var addedCustomer = await _customersRepository.AddAsync(receivedCustomer);

                addedCustomer.password = null;

                return CreatedAtAction(nameof(Get),
                    new { email = addedCustomer.email },
                    new { message = "Success", customer = addedCustomer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpPatch("{email}", Name = "UpdateCustomer")]
        [Authorize]
        public async Task<IActionResult> Patch(string email, Customers customerUpdate)
        {
            try
            {
                var existingCustomer = await _customersRepository.GetByEmailAsync(email);
                if (existingCustomer == null)
                {
                    return BadRequest(new { message = "Customer not found" });
                }

                existingCustomer.firstname = customerUpdate.firstname ?? existingCustomer.firstname;
                existingCustomer.lastname = customerUpdate.lastname ?? existingCustomer.lastname;
                existingCustomer.mobile_number = customerUpdate.mobile_number ?? existingCustomer.mobile_number;
                existingCustomer.delivery_adress = customerUpdate.delivery_adress ?? existingCustomer.delivery_adress;

                await _customersRepository.UpdateAsync(existingCustomer);
                return Ok(new { message = "Success", customer = existingCustomer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }

        [HttpDelete("{email}", Name = "DeleteCustomer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                var customer = await _customersRepository.GetByEmailAsync(email);
                if (customer == null)
                {
                    return BadRequest(new { message = "Customer not found" });
                }
                await _customersRepository.DeleteAsync(customer);
                return Ok(new { message = "Success", customer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ett internt fel har inträffat");
            }
        }
    }
}
