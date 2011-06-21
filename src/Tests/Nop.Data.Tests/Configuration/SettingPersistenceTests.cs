﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Nop.Tests;
using Nop.Core.Domain;
using Nop.Core.Domain.Configuration;

namespace Nop.Data.Tests.Configuration
{
    [TestFixture]
    public class SettingPersistenceTests : PersistenceTest
    {
        [Test]
        public void Can_save_and_load_setting()
        {
            var setting = new Setting
            {
                Name = "Setting1",
                Value = "Value1",
            };

            var fromDb = SaveAndLoadEntity(setting);
            fromDb.ShouldNotBeNull();
            fromDb.Name.ShouldEqual("Setting1");
            fromDb.Value.ShouldEqual("Value1");
        }
    }
}