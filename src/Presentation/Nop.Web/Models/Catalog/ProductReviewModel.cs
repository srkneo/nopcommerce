﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Media;
using Nop.Web.Validators.Catalog;
using Nop.Web.Validators.Common;

namespace Nop.Web.Models.Catalog
{
    public class ProductReviewOverviewModel : BaseNopModel
    {
        public int ProductId { get; set; }

        public int RatingSum { get; set; }

        public int TotalReviews { get; set; }

        public bool AllowCustomerReviews { get; set; }

        public bool CustomerIsRegistered { get; set; }

        public bool AllowAnonymousUsersToReviewProduct { get; set; }
    }

    [Validator(typeof(ProductReviewsValidator))]
    public class ProductReviewsModel : BaseNopModel
    {
        public ProductReviewsModel()
        {
            Items = new List<ProductReviewModel>();
            AddProductReview = new AddProductReviewModel();
        }
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSeName { get; set; }

        public IList<ProductReviewModel> Items { get; set; }
        public AddProductReviewModel AddProductReview { get; set; }
    }

    public class ProductReviewModel : BaseNopEntityModel
    {
        public string CustomerName { get; set; }
        
        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }

        public string WrittenOnStr { get; set; }
    }

    public class AddProductReviewModel : BaseNopModel
    {
        [AllowHtml]
        [NopResourceDisplayName("Reviews.Fields.Title")]
        public string Title { get; set; }

        [AllowHtml]
        [NopResourceDisplayName("Reviews.Fields.ReviewText")]
        public string ReviewText { get; set; }

        [NopResourceDisplayName("Reviews.Fields.Rating")]
        public int Rating { get; set; }

        public bool CustomerIsRegistered { get; set; }

        public bool AllowAnonymousUsersToReviewProduct { get; set; }

        public bool SuccessfullyAdded { get; set; }
        public string Result { get; set; }
    }
}