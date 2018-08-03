using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Basket.Library;
using FluentAssertions;

namespace Basket.Library.Tests
{
    [TestClass]
    public class BasketTest
    {
        private Product Hat;
        private Product Jumper;
        private Product HeadLight;

        private GiftVoucher Voucher1;
        private OfferVoucher OfferVoucher1;
        private OfferVoucher OfferVoucherAppliesToHeadGear;

        private Cart Cart1;
        private Cart CartWith2Hats;
        private Cart CartWith2HatsAnd1Jumper;

        [TestInitialize]
        public void SetUp()
        {
            Hat = new Product("Hat", 10.5, new Category(""));
            Jumper = new Product("Jumper", 54.56, new Category(""));
            HeadLight = new Product("Head Light", 3.5, new Category("Head Gear"));
            Cart1 = new Cart(new List<CartItem>() { new CartItem(Hat, 1) } );
            CartWith2Hats = new Cart(new List<CartItem>() { new CartItem(Hat, 2) } );;
            CartWith2HatsAnd1Jumper = new Cart(new List<CartItem>() { new CartItem(Hat, 2), new CartItem(Jumper, 1) } );

            Voucher1 = new GiftVoucher("XXX-XXX", 5.0);
            OfferVoucher1 = new OfferVoucher("YYY-YYY", 5.0);
            OfferVoucherAppliesToHeadGear = new OfferVoucher("YYY-YYY", 5.0, 50.00, new Category("Head Gear"));
        }


        /// <summary>
        /// Basket 5:
        /// 1 Hat @ £25.00
        /// 1 £30 Gift Voucher @ £30.00
        /// ------------
        /// 1 x £5.00 off baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £55.00
        /// ------------
        /// </summary>
        [TestMethod]
        [CustomAssertion]
        public void Basket_whenOfferVoucherIsAppliedAndTreshholdreached_return_Total()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 25.00), 1);
            Product product2 = cart.Add(new Product("£30 Gift Voucher", 30.00), 1);
            product2.BlockVouchers = true;
            OfferVoucher offerVoucher = new OfferVoucher("YYY-YYY", 5.00, 50.00);
            Basket basket = new Basket(cart);
            basket.AddVoucher(offerVoucher);
            double thresholdReached = basket.ThresholdReached;
            Assert.AreEqual(55.00, basket.Total);
            thresholdReached.Should().Equals(25.01);
             // .BeTrue("You have not reached the spend threshold for voucher YYY-YYY. Spend another £25.01 to receive £5.00 discount from your basket total.");
        }

        /// <summary>
        /// Basket 4:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £26.00
        /// ------------
        /// 1 x £5.00 Gift Voucher XXX-XXX applied
        /// 1 x £5.00 off baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £41.00
        /// ------------
        /// </summary>
        [TestMethod]
        [CustomAssertion]
        public void Basket_when1GiftVoucherAnd1OfferVoucherIsApplied_return_Total()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 25.00), 1);
            Product product2 = cart.Add(new Product("Jumper", 26.00), 1);
            GiftVoucher giftVoucher = new GiftVoucher("XXX-XXX", 5.00);
            OfferVoucher offerVoucher = new OfferVoucher("YYY-YYY", 5.00, 50.00);
            Basket basket = new Basket(cart);
            basket.AddVoucher(giftVoucher);
            basket.AddVoucher(offerVoucher);
            bool IsCodeValid1 = basket.ApplyVoucher("XXX-XXX");
            bool IsCodeValid2 = basket.ApplyVoucher("YYY-YYY");
            Assert.AreEqual(41.00, basket.Total);
        }

        /// <summary>
        /// Basket 3:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £26.00
        /// 1 Head Light (Head Gear Category of Product)  @ £3.50
        /// ------------
        /// 1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £51.00
        /// ------------
        /// </summary>
        [TestMethod]
        [CustomAssertion]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppledTiCategoryAppliedAndCartHasItemInCategoryAndHasMinSpend_return_Total()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 25.00), 1);
            Product product2 = cart.Add(new Product("Jumper", 26.00), 1);
            Product product3 = cart.Add(new Product("Head Light", 3.50, new Category("Head Gear")), 1);
            OfferVoucher expected = new OfferVoucher("YYY-YYY", 5.00, 50.00, new Category("Head Gear"));
            Basket basket = new Basket(cart, expected);
            bool IsCodeValid = basket.ApplyVoucher("YYY-YYY");
            Assert.AreEqual(51.00, basket.Total);
            IsCodeValid.Should().BeTrue();
        }


        /// <summary>
        /// Basket 2:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £26.00
        /// ------------
        /// 1 x £5.00 off Head Gear in baskets over £50.00 Offer Voucher YYY-YYY applied
        /// ------------
        /// Total: £51.00
        /// ------------
        /// </summary>
        [TestMethod]
        [CustomAssertion]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppledTiCategoryAppliedAndCartHasItemInCategory_return_Total()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 25.00), 1);
            Product product3 = cart.Add(new Product("Jumper", 26.00), 1);
            OfferVoucher expected = new OfferVoucher("YYY-YYY", 5.00, 50.00, new Category("Head Gear"));
            Basket basket = new Basket(cart, expected);
            bool IsCodeValid = basket.ApplyVoucher("YYY-YYY");
            Assert.AreEqual(51.00, basket.Total);
            IsCodeValid.Should().BeFalse("There are no products in your basket applicable to voucher Voucher YYY-YYY");
        }

        /// <summary>
        /// Basket 2:
        /// 1 Hat @ £25.00
        /// 1 Jumper @ £54.65
        /// ------------
        /// 1 x 5.00 Gift Voucher XXX-XXX applied
        /// ------------
        /// Total: £60.15
        /// ------------
        /// </summary>
        [TestMethod]
        [CustomAssertion]
        public void Basket_whenGiftVoucherAddedAndGiftCodeAppliedAndCodeIsValid_return_BasketTotal()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 10.50), 1);
            Product product3 = cart.Add(new Product("Jumper", 54.65), 1);
            GiftVoucher giftVoucher = new GiftVoucher("XXX-XXX", 5.00);
            Basket basket = new Basket(cart, giftVoucher);
            bool applied = basket.ApplyVoucher("XXX-XXX");
            basket.Total.Should().Equals(60.15);
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppliedAndCodeIsInValid_return_BasketTotal()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            Basket basket = new Basket(cart);
            Assert.AreEqual(10.5, basket.Total);
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppledTiCategoryAppliedAndCartHasItemInCategory_return_CondValid()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Head Light", 10.5, new Category("Head Gear")), 1) });
            OfferVoucher expected = new OfferVoucher("XXX-XXX", 5.00, 50.00, new Category("Head Gear"));
            Basket basket = new Basket(cart, expected);
            Assert.IsTrue(basket.ApplyVoucher("XXX-XXX"));
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppledTiCategoryAppliedAndCartHasItemNotInCategory_return_CondNotValid()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5), 1) });
            OfferVoucher expected = new OfferVoucher("XXX-XXX", 5.00, 50.00, new Category("Head Gear"));
            Basket basket = new Basket(cart, expected);
            Assert.IsFalse(basket.ApplyVoucher("XXX-XXX"));
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppliedAndCodeIsInValid_return_ValidCode()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            OfferVoucher offerVoucher = new OfferVoucher("XXX-XXX", 5);
            Basket basket = new Basket(cart, offerVoucher);
            Assert.IsFalse(basket.ApplyVoucher("YYY-YYY"));
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppliedAndCodeIsInValid_return_CodeValidCode()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            Basket basket = new Basket(cart);
            Assert.IsFalse(basket.ApplyVoucher("XXX-XXX"));
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAddedAndOffCodeAppliedAndCodeIsValid_return_ValidCode()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            OfferVoucher offerVoucher = new OfferVoucher("XXX-XXX", 5);
            Basket basket = new Basket(cart, offerVoucher);
            Assert.IsTrue(basket.ApplyVoucher("XXX-XXX"));
        }

        [TestMethod]
        public void Basket_whenGiftVoucherAddedAndGiftCodeAppliedAndCodeIsValid_return_ValidCode()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            GiftVoucher giftVoucher = new GiftVoucher("XXX-XXX", 5);
            Basket basket = new Basket(cart, giftVoucher);
            Assert.IsTrue(basket.ApplyVoucher("XXX-XXX"));
        }

        [TestMethod]
        public void Basket_whenGiftVoucherAddedAndGiftAppliedAndNoVoucherIsPresent_return_CodeNotValid()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            Basket basket = new Basket(cart);
            Assert.IsFalse(basket.ApplyVoucher("XXX-XXX"));
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAttachedToCategoryAdded_return_BasketHasVoucher()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            OfferVoucher expected = new OfferVoucher("XXX-XXX", 5.00, 50.00, new Category("Head Gear"));
            Basket basket = new Basket(cart, expected);
            Assert.IsTrue(basket.HasVouchers);
        }

        [TestMethod]
        public void Basket_whenOfferVoucherAdded_return_BasketHasVoucher()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            OfferVoucher offerVoucher = new OfferVoucher("XXX-XXX", 5);
            Basket basket = new Basket(cart, offerVoucher);
            Assert.IsTrue(basket.HasVouchers);
        }

        [TestMethod]
        public void Basket_whenGiftVoucherAdded_return_BasketHasVoucher()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            GiftVoucher giftVoucher = new GiftVoucher("XXX-XXX", 5);
            Basket basket = new Basket(cart, giftVoucher);
            Assert.IsTrue(basket.HasVouchers);
        }

        [TestMethod]
        public void Basket_WhenItemAddedToCar_return_BasketIsNotEmpty()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            Basket basket = new Basket(cart);
            Assert.IsFalse(basket.IsEmpty);
        }

        [TestMethod]
        [CustomAssertion]
        public void Cart_when2ofSameProductsAnd1DiffernetProductAdded_return_2Items()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product2 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product3 = cart.Add(new Product("Jumper", 54.56, new Category("")), 1);
            CartWith2HatsAnd1Jumper.List.Should().BeEquivalentTo(cart.List);
        }

        [TestMethod]
        [CustomAssertion]
        public void Cart_when2ofSameProductsAdded_return_SingleItemWithQuantity2()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product2 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            CartWith2Hats.List.Should().BeEquivalentTo(cart.List);
        }

        [TestMethod]
        public void Cart_when2ofSameProductsAnd1DiffernetProductAdded_return_CartTotal()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product2 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product3 = cart.Add(new Product("Jumper", 54.56, new Category("")), 1);
            Assert.AreEqual(75.56, cart.Total);
        }

        [TestMethod]
        public void Cart_When2ProductsAdded_return_CartTotal()
        {
            Cart cart = new Cart();
            Product expected = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product2 = cart.Add(new Product("Jumper", 54.65, new Category("")),1);
            Assert.AreEqual(65.15, cart.Total);
        }

        [TestMethod]
        public void Cart_When1HatProductAdded_return_CartTotal()
        {
            Cart cart = new Cart();
            Product expected = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Assert.AreEqual(10.5, cart.Total);
        }

        [TestMethod]
        public void Cart_whenAdding2ItemUsingAddMethod_return_TotalItem2()
        {
            Cart cart = new Cart();
            Product product1 = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Product product2 = cart.Add(new Product("Jumper", 54.65, new Category("")), 1);
            Assert.AreEqual(2, cart.NoOfItems);
        }

        [TestMethod]
        public void Cart_whenAdding1ItemUsingAddMethod_return_TotalItem1()
        {
            Cart cart = new Cart();
            Product expected = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            Assert.AreEqual(1, cart.NoOfItems);
        }

        [TestMethod]
        [CustomAssertion]
        public void Cart_whenAddingSingleItem_return_item()
        {
            Cart cart = new Cart();
            Product expected = cart.Add(new Product("Hat", 10.5, new Category("")), 1);
            expected.Should().BeEquivalentTo(Hat);
        }

        [TestMethod]
        public void Cart_when2Product2Added_return_TotalItems2()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1), new CartItem(new Product("Jumper", 10.5, new Category("")), 1)});
            Assert.AreEqual(2, cart.NoOfItems);
        }

        [TestMethod]
        public void Cart_when1ProductAdded_return_TotalItems1()
        {
            Cart cart = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            Assert.AreEqual(1, cart.NoOfItems);
        }

        [TestMethod]
        [CustomAssertion]
        public void Voucher_WhenGiftVoucherInitialised_return_NewVoucher()
        {
            GiftVoucher expected = new GiftVoucher("XXX-XXX", 5);
            expected.Should().BeEquivalentTo(Voucher1);
        }

        [TestMethod]
        [CustomAssertion]
        public void Voucher_WhenOfferVoucherInitialised_return_NewVoucher()
        {
            OfferVoucher expected = new OfferVoucher("YYY-YYY", 5);
            expected.Should().BeEquivalentTo(OfferVoucher1);
        }

        [TestMethod]
        [CustomAssertion]
        public void Voucher_WhenOfferVoucherInitialised_return_NotGiftVoucher()
        {
            GiftVoucher giftVoucher = new GiftVoucher("YYY-YYY", 5);
            OfferVoucher expected = new OfferVoucher("YYY-YYY", 5);
            expected.Should().NotBe(giftVoucher, "Gift vouchers have different rules to offer vouchers");
        }

        [TestMethod]
        [CustomAssertion]
        public void Product_When1ItemInitialised_return_Product()
        {
            Product expected = new Product("Hat", 10.5, new Category(""));
            expected.Should().BeEquivalentTo(Hat);
        }

        [TestMethod]
        [CustomAssertion]
        public void Cart_When1Initialised_return_Cart()
        {
            Cart expected = new Cart(new List<CartItem>() { new CartItem(new Product("Hat", 10.5, new Category("")), 1) });
            expected.Should().BeEquivalentTo(Cart1);
        }
    }
}
