namespace WebDev_Labb2_API.Model
{
    public class CustomerMethods
    {
        public Customers CreateCustomer(Customers newCustomer)
        {
            var deliveryadress = new DeliveryAddress
            {
                street = newCustomer.delivery_adress.street,
                post_code = newCustomer.delivery_adress.post_code,
                city = newCustomer.delivery_adress.city,
                country = "Sweden"
            };
            var customer = new Customers
            {
                firstname = newCustomer.firstname,
                lastname = newCustomer.lastname,
                email = newCustomer.email,
                mobile_number = newCustomer.mobile_number,
                userlevel = "customer",
                username = newCustomer.username,
                password = newCustomer.password,
                delivery_adress = deliveryadress
            };

            return customer;
        }
    }
}
