﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Domain.Customers;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Tax;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class ShoppingCartItemPeristenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_shoppingCartItem()
        {
            var sci = new ShoppingCartItem
            {
                ShoppingCartType = ShoppingCartType.ShoppingCart,
                AttributesXml = "AttributesXml 1",
                CustomerEnteredPrice = 1.1M,
                Quantity= 2,
                CreatedOnUtc = new DateTime(2010, 01, 01),
                UpdatedOnUtc = new DateTime(2010, 01, 02),
                Customer = GetTestCustomer(),
                ProductVariant = GetTestProductVariant()
            };

            var fromDb = SaveAndLoadEntity(sci);
            fromDb.ShouldNotBeNull();

            fromDb.ShoppingCartType.ShouldEqual(ShoppingCartType.ShoppingCart);
            fromDb.AttributesXml.ShouldEqual("AttributesXml 1");
            fromDb.CustomerEnteredPrice.ShouldEqual(1.1M);
            fromDb.Quantity.ShouldEqual(2);
            fromDb.CreatedOnUtc.ShouldEqual(new DateTime(2010, 01, 01));
            fromDb.UpdatedOnUtc.ShouldEqual(new DateTime(2010, 01, 02));

            fromDb.Customer.ShouldNotBeNull();

            fromDb.ProductVariant.ShouldNotBeNull();

        }

        protected Customer GetTestCustomer()
        {
            return new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                AdminComment = "some comment here",
                TaxDisplayType = TaxDisplayType.IncludingTax,
                IsTaxExempt = true,
                VatNumber = "123456",
                VatNumberStatus = VatNumberStatus.Valid,
                DiscountCouponCode = "coupon1",
                Active = true,
                Deleted = false,
                CreatedOnUtc = new DateTime(2010, 01, 01),
                LastActivityDateUtc = new DateTime(2010, 01, 02)
            };
        }

        protected ProductVariant GetTestProductVariant()
        {
            return new ProductVariant
            {
                Name = "Product variant name 1",
                CreatedOnUtc = new DateTime(2010, 01, 03),
                UpdatedOnUtc = new DateTime(2010, 01, 04),
                Product = new Product()
                {
                    Name = "Name 1",
                    Published = true,
                    Deleted = false,
                    CreatedOnUtc = new DateTime(2010, 01, 01),
                    UpdatedOnUtc = new DateTime(2010, 01, 02)
                }
            };
        }
    }
}
