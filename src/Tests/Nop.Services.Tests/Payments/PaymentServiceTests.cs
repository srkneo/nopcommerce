﻿using System.Collections.Generic;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Infrastructure;
using Nop.Core.Plugins;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Tests;
using NUnit.Framework;
using Rhino.Mocks;

namespace Nop.Services.Tests.Payments
{
    [TestFixture]
    public class PaymentServiceTests
    {
        PaymentSettings _paymentSettings;
        IPaymentService _paymentService;
        
        [SetUp]
        public void SetUp()
        {
            _paymentSettings = new PaymentSettings();
            _paymentSettings.ActivePaymentMethodSystemNames = new List<string>();
            _paymentSettings.ActivePaymentMethodSystemNames.Add("Payments.TestMethod");

            var pluginFinder = new PluginFinder(new AppDomainTypeFinder());
            _paymentService = new PaymentService(_paymentSettings, pluginFinder);
        }

        [Test]
        public void Can_load_paymentMethods()
        {
            var srcm = _paymentService.LoadActivePaymentMethods();
            srcm.ShouldNotBeNull();
            (srcm.Count > 0).ShouldBeTrue();
        }

        [Test]
        public void Can_load_paymentMethod_by_systemKeyword()
        {
            var srcm = _paymentService.LoadPaymentMethodBySystemName("Payments.TestMethod");
            srcm.ShouldNotBeNull();
        }

        [Test]
        public void Can_load_active_paymentMethods()
        {
            var srcm = _paymentService.LoadActivePaymentMethods();
            srcm.ShouldNotBeNull();
            (srcm.Count > 0).ShouldBeTrue();
        }

        [Test]
        public void Can_get_masked_credit_card_number()
        {
            _paymentService.GetMaskedCreditCardNumber("").ShouldEqual("");
            _paymentService.GetMaskedCreditCardNumber("123").ShouldEqual("123");
            _paymentService.GetMaskedCreditCardNumber("1234567890123456").ShouldEqual("************3456");
        }
    }
}