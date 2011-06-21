﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nop.Core.Domain.Security.Permissions;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain;
using Nop.Core.Domain.Customers;

namespace Nop.Data.Tests.Customers
{
    [TestFixture]
    public class CustomerRolePersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_customerRole()
        {
            var customerRole = GetTestCustomerRole();

            var fromDb = SaveAndLoadEntity(customerRole);
            fromDb.ShouldNotBeNull();
            fromDb.Name.ShouldEqual("Administrators");
            fromDb.FreeShipping.ShouldEqual(true);
            fromDb.TaxExempt.ShouldEqual(true);
            fromDb.Active.ShouldEqual(true);
            fromDb.IsSystemRole.ShouldEqual(true);
            fromDb.SystemName.ShouldEqual("Administrators");
        }

        [Test]
        public void Can_save_and_load_customerRole_with_customer()
        {
            var customerRole = GetTestCustomerRole();
            customerRole.Customers.Add(
                new Customer()
                {
                    CustomerGuid = Guid.NewGuid(),
                    AdminComment = "some comment here",
                    Active = true,
                    CreatedOnUtc = new DateTime(2010, 01, 01)
                }
            );

            var fromDb = SaveAndLoadEntity(customerRole);
            fromDb.ShouldNotBeNull();
            fromDb.Name.ShouldEqual("Administrators");

            fromDb.Customers.ShouldNotBeNull();
            (fromDb.Customers.Count == 1).ShouldBeTrue();
        }

        [Test]
        public void Can_save_and_load_customerRole_with_permissions()
        {
            var customerRole = GetTestCustomerRole();
            customerRole.PermissionRecords.Add
            (
                new PermissionRecord()
                {
                    Name = "Name 1",
                    SystemName = "SystemName 2",
                    Category = "Category 4",
                }
            );

            var fromDb = SaveAndLoadEntity(customerRole);
            fromDb.ShouldNotBeNull();
            fromDb.Name.ShouldEqual("Administrators");

            fromDb.PermissionRecords.ShouldNotBeNull();
            (fromDb.PermissionRecords.Count == 1).ShouldBeTrue();
            fromDb.PermissionRecords.First().Name.ShouldEqual("Name 1");
        }

        protected CustomerRole GetTestCustomerRole()
        {
            return new CustomerRole
            {
                Name = "Administrators",
                FreeShipping = true,
                TaxExempt = true,
                Active = true,
                IsSystemRole = true,
                SystemName = "Administrators"
            };
        }
    }
}