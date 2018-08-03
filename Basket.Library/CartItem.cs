namespace Basket.Library
{
    public class CartItem
    {
        private Product _product;
        private int _quantity;

        public CartItem(Product product, int quantity)
        {
            this._product = product;
            this._quantity = quantity;
        }

        public Product Product
        {
            get
            {
                return _product;
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }
    }
}