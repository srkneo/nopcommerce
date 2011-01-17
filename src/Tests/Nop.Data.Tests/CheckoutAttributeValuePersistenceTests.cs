﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class CheckoutAttributeValuePersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_checkoutAttributeValue()
        {
            var cav = new CheckoutAttributeValue()
                    {
                        Name = "Name 2",
                        PriceAdjustment= 1, 
                        WeightAdjustment = 2,
                        IsPreSelected = true,
                        DisplayOrder = 3,
                        CheckoutAttribute = new CheckoutAttribute
                        {
                            Name = "Name 1",
                            TextPrompt = "TextPrompt 1",
                            IsRequired = true,
                            ShippableProductRequired = true,
                            IsTaxExempt = true,
                            TaxCategoryId = 1,
                            AttributeControlType = AttributeControlType.Datepicker,
                            DisplayOrder = 2
                        }
                    };

            var fromDb = SaveAndLoadEntity(cav);
            fromDb.ShouldNotBeNull();
            fromDb.Name.ShouldEqual("Name 2");

            fromDb.CheckoutAttribute.ShouldNotBeNull();
            fromDb.CheckoutAttribute.Name.ShouldEqual("Name 1");
        }
    }
}