﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Tests
{
    [TestFixture]
    public class SpecificationAttributeOptionPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_specificationAttributeOption()
        {
            var specificationAttributeOption = new SpecificationAttributeOption
            {
                Name = "SpecificationAttributeOption name 1",
                DisplayOrder = 1,
                SpecificationAttribute = new SpecificationAttribute()
                {
                    Name = "SpecificationAttribute name 1",
                    DisplayOrder = 2,
                }
            };

            var fromDb = SaveAndLoadEntity(specificationAttributeOption);
            fromDb.ShouldNotBeNull();
            fromDb.Name.ShouldEqual("SpecificationAttributeOption name 1");
            fromDb.DisplayOrder.ShouldEqual(1);

            fromDb.SpecificationAttribute.ShouldNotBeNull();
            fromDb.SpecificationAttribute.Name.ShouldEqual("SpecificationAttribute name 1");
        }
    }
}
