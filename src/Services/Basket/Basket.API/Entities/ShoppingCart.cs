namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShopingCartItem> Items { get; set; } = new List<ShopingCartItem>();

        public ShoppingCart() { }

        public ShoppingCart(string name) 
        {
            UserName = name;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                totalPrice = Items.Sum(x => x.Price * x.Quantity);
                return totalPrice;
            }
        }
    }
}
