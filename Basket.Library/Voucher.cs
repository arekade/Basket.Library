using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Library
{
    public abstract class Voucherbase
    {
        private double _amount;
        private string _code;
        private VoucherType _type;
        private Category _appliesTo;
        private double _basketMinimum;

        public Voucherbase()
        {

        }

        public Voucherbase(VoucherType type)
        {
            this._type = type;
        }

        public Voucherbase(string code, double amount)
        {
            this._code = code;
            this._amount = amount;
        }

        public string Code { get => _code;  set => _code = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public double BasketMinimum { get => _basketMinimum; set => _basketMinimum = value; }
        public VoucherType Type { get => _type; set => _type = value; }
        public Category AppliesTo  { get => _appliesTo; set => _appliesTo = value; }
}

    public class GiftVoucher : Voucherbase
    {
        private const VoucherType type = VoucherType.Gift;
        public GiftVoucher()
        {
            Type = type;
        }

        public GiftVoucher(string code, double amount)
        {
            Type = type;
            Code = code;
            Amount = amount;
        }
    }
    public class OfferVoucher : Voucherbase
    {
        private const VoucherType type = VoucherType.Offer;

        public OfferVoucher()
        {
            Type = type;
        }

        public OfferVoucher(string code, double amount)
        {
            Type = type;
            Code = code;
            Amount = amount;
        }

        public OfferVoucher(string code, double amount, double basketMinimum, Category appliesTo)
        {
            Type = type;
            Code = code;
            Amount = amount;
            AppliesTo = appliesTo;
            BasketMinimum = basketMinimum;
        }

        public OfferVoucher(string code, double amount, double basketMinimum)
        {
            Type = type;
            Code = code;
            Amount = amount;
            BasketMinimum = basketMinimum;
        }


    }

}
