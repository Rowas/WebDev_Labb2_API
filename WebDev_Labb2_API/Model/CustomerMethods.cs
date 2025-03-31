using Microsoft.AspNetCore.Identity;

namespace WebDev_Labb2_API.Model
{
    public class CustomerMethods
    {
        private PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        public Customers CreateCustomer(Customers newCustomer)
        {
            newCustomer.password = passwordHasher.HashPassword(newCustomer.username, newCustomer.password);

            var deliveryadress = new DeliveryAddress
            {
                street = newCustomer.delivery_adress.street,
                post_code = newCustomer.delivery_adress.post_code,
                city = newCustomer.delivery_adress.city,
                country = newCustomer.delivery_adress.country
            };
            var customer = new Customers
            {
                firstname = newCustomer.firstname,
                lastname = newCustomer.lastname,
                email = newCustomer.email,
                mobile_number = newCustomer.mobile_number,
                userlevel = newCustomer.userlevel,
                username = newCustomer.username,
                password = newCustomer.password,
                delivery_adress = deliveryadress
            };

            return customer;
        }

        public PasswordVerificationResult VerifyLogin(Customers customer, string password)
        {
            return passwordHasher.VerifyHashedPassword(customer.username, customer.password, password);
        }
    }
}
