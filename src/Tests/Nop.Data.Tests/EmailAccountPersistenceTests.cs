﻿using System;
using System.Collections.Generic;
using System.Linq;

using Nop.Core.Domain.Messages;
using Nop.Tests;

using NUnit.Framework;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class EmailAccountPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_emailAccount()
        {
            var emailAccount = new EmailAccount
            {
                Email = "admin@yourstore.com",
                DisplayName = "Administrator",
                Host = "127.0.0.1",
                Port = 125,
                Username = "John",
                Password = "111",
                EnableSSL = true,
                UseDefaultCredentials = true
            };

            var fromDb = SaveAndLoadEntity(emailAccount);
            fromDb.ShouldNotBeNull();
            fromDb.Email.ShouldEqual("admin@yourstore.com");
            fromDb.DisplayName.ShouldEqual("Administrator");
            fromDb.Host.ShouldEqual("127.0.0.1");
            fromDb.Port.ShouldEqual(125);
            fromDb.Username.ShouldEqual("John");
            fromDb.Password.ShouldEqual("111");
            fromDb.EnableSSL.ShouldBeTrue();
            fromDb.UseDefaultCredentials.ShouldBeTrue();
        }

        [Test]
        public void Can_save_and_load_emailAccount_with_messageTemplates()
        {
            var emailAccount = new EmailAccount
            {
                Email = "admin@yourstore.com",
                DisplayName = "Administrator",
                Host = "127.0.0.1",
                Port = 125,
                Username = "John",
                Password = "111",
                EnableSSL = true,
                UseDefaultCredentials = true,
                MessageTemplates = new List<MessageTemplate>()
                {
                    new MessageTemplate()
                    {
                        Name = "Template1",
                        BccEmailAddresses = "Bcc",
                        Subject = "Subj",
                        Body = "Some text",
                        IsActive = true                        
                    }
                }
            };


            var fromDb = SaveAndLoadEntity(emailAccount);
            fromDb.ShouldNotBeNull();
            fromDb.Email.ShouldEqual("admin@yourstore.com");

            fromDb.MessageTemplates.ShouldNotBeNull();
            (fromDb.MessageTemplates.Count == 1).ShouldBeTrue();
            fromDb.MessageTemplates.First().Name.ShouldEqual("Template1");
        }

        [Test]
        public void Can_save_and_load_emailAccount_with_queuedEmails()
        {
            var emailAccount = new EmailAccount
            {
                Email = "admin@yourstore.com",
                DisplayName = "Administrator",
                Host = "127.0.0.1",
                Port = 125,
                Username = "John",
                Password = "111",
                EnableSSL = true,
                UseDefaultCredentials = true,
                QueuedEmails = new List<QueuedEmail>()
                {
                    new QueuedEmail()
                    {
                        Priority = 1,
                        From = "From",
                        FromName = "FromName",
                        To = "To",
                        ToName = "ToName",
                        CC = "CC",
                        Bcc = "Bcc",
                        Subject = "Subject",
                        Body = "Body",
                        CreatedOnUtc = new DateTime(2010, 01, 01)
                    }
                }
            };


            var fromDb = SaveAndLoadEntity(emailAccount);
            fromDb.ShouldNotBeNull();
            fromDb.Email.ShouldEqual("admin@yourstore.com");

            fromDb.MessageTemplates.ShouldNotBeNull();
            (fromDb.QueuedEmails.Count == 1).ShouldBeTrue();
            fromDb.QueuedEmails.First().From.ShouldEqual("From");
        }

    }
}
