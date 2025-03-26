using MongoDB.EntityFrameworkCore;

namespace WebDev_Labb2_API.Model
{
    [Collection("Orders")]
    public class Orders
    {
        public string username { get; set; }
        public string order_id { get; set; }
        public string order_date { get; set; }
        public string delivery_date { get; set; }
        public string status { get; set; }
        public List<ItemList> item_list { get; set; }
    }

    public class ItemList
    {
        public int sku { get; set; }
        public int quantity { get; set; }
    }
}
