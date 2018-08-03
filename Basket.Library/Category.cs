using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Library
{
    public class Category
    {
        private readonly string _title;

        public Category() { }

        public Category(string Title)
        {
            this._title = Title;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }
    }
}
