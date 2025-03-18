using MongoDB.EntityFrameworkCore;

namespace WebDev_Labb2_API.Model
{
    [Collection("Customers")]
    public class Customers
    {
        public string UserName { get; set; }
        public string UserLevel { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string mobileNumber { get; set; }
        public DeliveryAddress deliveryAddress { get; set; }
    }

    public class DeliveryAddress
    {
        public string street { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
}
