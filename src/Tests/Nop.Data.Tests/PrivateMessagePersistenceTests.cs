﻿using System;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Tests;
using NUnit.Framework;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class PrivateMessagePersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_privatemessage()
        {
            var customer1 = GetTestCustomer("1");
            var customer1FromDb = SaveAndLoadEntity(customer1);
            customer1FromDb.ShouldNotBeNull();

            var customer2 = GetTestCustomer("2");
            var customer2FromDb = SaveAndLoadEntity(customer2);
            customer2FromDb.ShouldNotBeNull();

            var privateMessage = new PrivateMessage
            {
                Subject = "Private Message 1 Subject",
                Text = "Private Message 1 Text",
                IsDeletedByAuthor = false,
                IsDeletedByRecipient = false,
                IsRead = false,
                CreatedOn = DateTime.Now,
                FromUserId = customer1FromDb.Id,
                ToUserId = customer2FromDb.Id,
            };

            var fromDb = SaveAndLoadEntity(privateMessage);
            fromDb.ShouldNotBeNull();
            fromDb.Subject.ShouldEqual("Private Message 1 Subject");
            fromDb.Text.ShouldEqual("Private Message 1 Text");
            fromDb.IsDeletedByAuthor.ShouldBeFalse();
            fromDb.IsDeletedByRecipient.ShouldBeFalse();
            fromDb.IsRead.ShouldBeFalse();
        }

        protected Customer GetTestCustomer(string number)
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
