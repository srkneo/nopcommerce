﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using FluentValidation.Attributes;

using Nop.Admin.Validators;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models
{
    public class GiftCardModel: BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.GiftCards.Fields.GiftCardType")]
        public GiftCardType GiftCardType { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.Order")]
        public int? PurchasedWithOrderId { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.Amount")]
        public decimal Amount { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.Amount")]
        public string AmountStr { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.RemainingAmount")]
        public string RemainingAmountStr { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.IsGiftCardActivated")]
        public bool IsGiftCardActivated { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.GiftCardCouponCode")]
        [AllowHtml]
        public string GiftCardCouponCode { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.RecipientName")]
        [AllowHtml]
        public string RecipientName { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.RecipientEmail")]
        [AllowHtml]
        public string RecipientEmail { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.SenderName")]
        [AllowHtml]
        public string SenderName { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.SenderEmail")]
        [AllowHtml]
        public string SenderEmail { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.Message")]
        [AllowHtml]
        public string Message { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.IsRecipientNotified")]
        public bool IsRecipientNotified { get; set; }

        [NopResourceDisplayName("Admin.GiftCards.Fields.CreatedOn")]
        public string CreatedOnStr { get; set; }
    }
}