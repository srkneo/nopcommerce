﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Domain;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Tests;
using NUnit.Framework;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class GiftCardUsageHistoryPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_giftCardUsageHistory()
        {
            var gcuh = new GiftCardUsageHistory()
                    {
                        UsedValue = 1.1M,
                        UsedValueInCustomerCurrency = 2.1M,
                        CreatedOnUtc = new DateTime(2010, 01, 01),
                        GiftCard = GetTestGiftCard(),
                        UsedWithOrder = GetTestOrder()
                    };

            var fromDb = SaveAndLoadEntity(gcuh);
            fromDb.ShouldNotBeNull();
            fromDb.UsedValue.ShouldEqual(1.1M);
            fromDb.UsedValueInCustomerCurrency.ShouldEqual(2.1M);
            fromDb.CreatedOnUtc.ShouldEqual(new DateTime(2010, 01, 01));

            fromDb.GiftCard.ShouldNotBeNull();
            fromDb.UsedWithOrder.ShouldNotBeNull();
        }


        protected Customer GetTestCustomer()
        {
            return new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = "admin@yourStore.com",
                Username = "admin@yourStore.com",
                AdminComment = "some comment here",
                Active = true,
                Deleted = false,
                CreatedOnUtc = new DateTime(2010, 01, 01)
            };
        }

        protected GiftCard GetTestGiftCard()
        {
           return new GiftCard()
            {
                Amount = 1,
                IsGiftCardActivated = true,
                GiftCardCouponCode = "Secret",
                RecipientName = "RecipientName 1",
                RecipientEmail = "a@b.c",
                SenderName = "SenderName 1",
                SenderEmail = "d@e.f",
                Message = "Message 1",
                IsRecipientNotified = true,
                CreatedOnUtc = new DateTime(2010, 01, 01),
            };
        }
        
        protected Order GetTestOrder()
        {
            return new Order
            {
                OrderGuid = Guid.NewGuid(),
                Customer = GetTestCustomer(),
                CreatedOnUtc = new DateTime(2010, 01, 01)
            };
        }
    }
}