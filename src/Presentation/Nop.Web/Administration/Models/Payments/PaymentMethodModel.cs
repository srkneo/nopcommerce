﻿using System.Web.Mvc;
using System.Web.Routing;
using Nop.Services.Payments;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Payments
{
    public class PaymentMethodModel : BaseNopModel
    {
        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.FriendlyName")]
        [AllowHtml]
        public string FriendlyName { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SystemName")]
        [AllowHtml]
        public string SystemName { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.Version")]
        [AllowHtml]
        public string Version { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.Author")]
        [AllowHtml]
        public string Author { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportCapture")]
        public bool SupportCapture { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportPartiallyRefund")]
        public bool SupportPartiallyRefund { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportRefund")]
        public bool SupportRefund { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportVoid")]
        public bool SupportVoid { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.RecurringPaymentType")]
        public RecurringPaymentType RecurringPaymentType { get; set; }
        



        public string ConfigurationActionName { get; set; }
        public string ConfigurationControllerName { get; set; }
        public RouteValueDictionary ConfigurationRouteValues { get; set; }
    }
}