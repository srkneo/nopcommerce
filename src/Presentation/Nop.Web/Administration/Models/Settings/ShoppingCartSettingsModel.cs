﻿using System.Web.Mvc;
using Nop.Core.Domain.Forums;
using Nop.Web.Framework;

namespace Nop.Admin.Models.Settings
{
    public class ShoppingCartSettingsModel
    {
        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.MaximumShoppingCartItems")]
        public int MaximumShoppingCartItems { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.MaximumWishlistItems")]
        public int MaximumWishlistItems { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.ShowProductImagesOnShoppingCart")]
        public bool ShowProductImagesOnShoppingCart { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.ShowProductImagesOnWishList")]
        public bool ShowProductImagesOnWishList { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.ShowDiscountBox")]
        public bool ShowDiscountBox { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.ShowGiftCardBox")]
        public bool ShowGiftCardBox { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.CrossSellsNumber")]
        public int CrossSellsNumber { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.WishlistEnabled")]
        public bool WishlistEnabled { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.ShoppingCart.EmailWishlistEnabled")]
        public bool EmailWishlistEnabled { get; set; }
    }
}