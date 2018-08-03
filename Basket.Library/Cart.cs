using System;
using System.Collections.Generic;

namespace Basket.Library
{
    public class Cart
    {
        private List<CartItem> _cartitems;

        public Cart() { }

        public Cart(List<CartItem> ProductList)
        {
            this._cartitems = ProductList;
        }

        public Product Add(Product product, int Quantity)
        {
            if (_cartitems == null)
            {
                _cartitems = new List<CartItem>();
            }

            CartItem item = new CartItem(product, Quantity);
            
            return addItemToCart(item).Product;
        }

        public CartItem Add(CartItem item)
        {
            if (_cartitems == null)
            {
                _cartitems = new List<CartItem>();
            }
            
            _cartitems.Add(item);
            return item;
        }

        public List<CartItem> List => _cartitems;

        public int NoOfItems
        {
            get => List.Count;
        }

        public double Total
        {
            get {
                double _total = 0;
                if (_cartitems != null)
                {
                    foreach (CartItem item in _cartitems)
                    {
                        _total += item.Product.Price * item.Quantity;
                    }
                }
                return _total;
            }
        }

        private CartItem addItemToCart(CartItem item)
        {
            if (_cartitems == null)
            {
                _cartitems = new List<CartItem>();
            }

            var found = false;
            _cartitems.ForEach(x =>
            {
                if (x.Product.Title == item.Product.Title)
                {
                    x.Quantity += item.Quantity;
                    found = true;
                }
            });

            if (!found)
            {
                _cartitems.Add(item);
            }
           
            return item;
        }
    }
}