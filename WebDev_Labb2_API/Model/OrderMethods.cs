namespace WebDev_Labb2_API.Model
{
    public class OrderMethods
    {
        public Orders CreateOrder(Orders newOrder)
        {
            var itemList = new ItemList
            {
                sku = newOrder.item_list[0].sku,
                quantity = newOrder.item_list[0].quantity
            };
            var order = new Orders
            {
                username = newOrder.username,
                order_id = newOrder.order_id,
                order_date = newOrder.order_date,
                delivery_date = newOrder.delivery_date,
                status = newOrder.status,
                item_list = newOrder.item_list
            };

            return order;
        }
    }
}
