﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Domain.Affiliates;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class AffiliatePersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_affiliate()
        {
            var affiliate = new Affiliate
            {
                Deleted = true,
                Active = true,
                Address = GetTestAddress(),
            };

            var fromDb = SaveAndLoadEntity(affiliate);
            fromDb.ShouldNotBeNull();
            fromDb.Deleted.ShouldEqual(true);
            fromDb.Active.ShouldEqual(true);
            fromDb.Address.ShouldNotBeNull();
            fromDb.Address.FirstName.ShouldEqual("FirstName 1");
        }

        [Test]
        public void Can_save_and_load_affiliate_with_customers()
        {
            var affiliate = new Affiliate
            {
                Deleted = true,
                Active = true,
                Address = GetTestAddress(),
            };
            affiliate.AffiliatedCustomers.Add(GetTestCustomer());

            var fromDb = SaveAndLoadEntity(affiliate);
            fromDb.ShouldNotBeNull();

            fromDb.AffiliatedCustomers.ShouldNotBeNull();
            (fromDb.AffiliatedCustomers.Count == 1).ShouldBeTrue();
            fromDb.AffiliatedCustomers.First().Active.ShouldEqual(true);
        }

        [Test]
        public void Can_save_and_load_affiliate_with_orders()
        {
            var affiliate = new Affiliate
            {
                Deleted = true,
                Active = true,
                Address = GetTestAddress(),
            };
            affiliate.AffiliatedOrders.Add(GetTestOrder());

            var fromDb = SaveAndLoadEntity(affiliate);
            fromDb.ShouldNotBeNull();

            fromDb.AffiliatedOrders.ShouldNotBeNull();
            (fromDb.AffiliatedOrders.Count == 1).ShouldBeTrue();
            fromDb.AffiliatedOrders.First().Deleted.ShouldEqual(true);
        }

        protected Address GetTestAddress()
        {
            return new Address()
            {
                FirstName = "FirstName 1",
                LastName = "LastName 1",
                Email = "Email 1",
                Company = "Company 1",
                City = "City 1",
                Address1 = "Address1a",
                Address2 = "Address1a",
                ZipPostalCode = "ZipPostalCode 1",
                PhoneNumber = "PhoneNumber 1",
                FaxNumber = "FaxNumber 1",
                CreatedOnUtc = new DateTime(2010, 01, 01),
                Country = new Country
                {
                    Name = "United States",
                    TwoLetterIsoCode = "US",
                    ThreeLetterIsoCode = "USA",
                }
            };
        }

        protected Customer GetTestCustomer()
        {
            return new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                AssociatedUserId = 4,
                AdminComment = "some comment here",
                TaxDisplayType = TaxDisplayType.IncludingTax,
                Active = true,
                Deleted = false,
                CreatedOnUtc = new DateTime(2010, 01, 01),
                LastActivityDateUtc = new DateTime(2010, 01, 02)
            };
        }

        protected Order GetTestOrder()
        {
            return new Order()
            {
                OrderGuid = Guid.NewGuid(),
                Customer = GetTestCustomer(),
                BillingAddress = new Address()
                {
                    Country = new Country()
                    {
                        Name = "United States",
                        TwoLetterIsoCode = "US",
                        ThreeLetterIsoCode = "USA",
                    },
                    CreatedOnUtc = new DateTime(2010, 01, 01),
                },
                Deleted = true,
                CreatedOnUtc = new DateTime(2010, 05, 06)
            };
        }
    }
}