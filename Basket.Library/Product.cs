using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Library
{
    public class Product : IProduct
    {
        private string _title;
        private double _price;
        private Category _category;
        private bool _blockvouchers = false;

        public Product()
        {
        }

        public Product(string Title, double Price)
        {
            this._title = Title;
            this._price = Price;
        }

        public Product(string Title, double Price, Category category)
        {
            this._title = Title;
            this._price = Price;
            this._category = category;
        }

        public Product Add(Product product)
        {
            return this;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }

        public double Price { get => _price; set => _price = value; }
        public bool BlockVouchers { get => _blockvouchers; set => _blockvouchers = value; }

        public Category Category
        {
            get
            {
                return _category;
            }
        }
    }
}
