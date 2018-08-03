using System;
using System.Collections.Generic;

namespace Basket.Library
{
    public class Basket
    {
        private Cart _cart;
        private List<Voucherbase> _vouchers;
        private double _basketTotal;
        private double _thresholdReached;

        public Basket()
        {
            _basketTotal = _cart.Total;
        }

        public Basket(Cart cart)
        {
            this._cart = cart;
            _basketTotal = _cart.Total;
        }

        public Basket(Cart cart, Voucherbase voucher)
        {
            this._cart = cart;
            AddVoucher(voucher);
            _basketTotal = _cart.Total;
        }

        public double Total => GetCartTotal();

        public bool IsEmpty => _cart == null || _cart.NoOfItems == 0;

        public bool HasVouchers => _vouchers != null && _vouchers.Count != 0;

        public bool ApplyVoucher(string code)
        {
            if (!HasVouchers)
            {
                return false;
            }
            else
            {
                bool found = false;
                _vouchers.ForEach(v =>
                {
                    if (v.Code == code)
                    {
                        var lastamount = _basketTotal;

                        _cart.List.ForEach(i =>
                        {
                            if (v.Type == VoucherType.Offer)
                            {
                                if (!i.Product.BlockVouchers)
                                {                                    
                                    if (v.AppliesTo != null)
                                    {
                                        if (i.Product.Category != null && i.Product.Category.Title == v.AppliesTo.Title)
                                        {
                                            found = true;
                                            if (_cart.Total >= v.BasketMinimum)
                                            {
                                                var orgprice = i.Product.Price;
                                                i.Product.Price = i.Product.Price > v.Amount ? i.Product.Price - v.Amount : 0;
                                                _basketTotal = _cart.Total;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        found = true;
                                        _basketTotal -= v.Amount;
                                        if (_basketTotal <= v.BasketMinimum)
                                        {
                                            _thresholdReached = lastamount - _basketTotal;
                                            _basketTotal = lastamount;
                                        }
                                    }

                                }
                            }
                            else
                            {

                                _basketTotal -= v.Amount;

                                found = true;
                            }
                        });
                    }

                });
                return found;
            }
        }

        public void AddVoucher(Voucherbase voucher)
        {
            if (_vouchers == null)
            {
                this._vouchers = new List<Voucherbase>();
            }
            this._vouchers.Add(voucher);
        }

        public double ThresholdReached => _thresholdReached;
        
        private double GetCartTotal()
        {
            if (!HasVouchers)
            {
                return _cart.Total;
            }
            else
            {
                return _basketTotal;
            }
        }
    }

    public enum VoucherType
    {
        Gift,
        Offer
    }
}
