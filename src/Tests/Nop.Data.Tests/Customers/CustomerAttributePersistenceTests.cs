﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Tests.Customers
{
    [TestFixture]
    public class CustomerAttributePersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_customerAttribute()
        {
            var customerAttribute = new CustomerAttribute
                               {
                                   Key = "Key 1",
                                   Value = "Value 1",
                                   Customer = GetTestCustomer()
                               };

            var fromDb = SaveAndLoadEntity(customerAttribute);
            fromDb.ShouldNotBeNull();
            fromDb.Key.ShouldEqual("Key 1");
            fromDb.Value.ShouldEqual("Value 1");

            fromDb.Customer.ShouldNotBeNull();
        }
        
        protected Customer GetTestCustomer()
        {
            return new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                AdminComment = "some comment here",
                Active = true,
                Deleted = false,
                CreatedOnUtc = new DateTime(2010, 01, 01)
            };
        }
    }
}