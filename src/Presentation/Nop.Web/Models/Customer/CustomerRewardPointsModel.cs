﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Core.Domain.Customers;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Customer
{
    public class CustomerRewardPointsModel : BaseNopModel
    {
        public CustomerRewardPointsModel()
        {
            RewardPoints = new List<RewardPointsHistoryModel>();
        }

        public IList<RewardPointsHistoryModel> RewardPoints { get; set; }
        public string RewardPointsBalance { get; set; }
        public CustomerNavigationModel NavigationModel { get; set; }

        #region Nested classes
        public class RewardPointsHistoryModel : BaseNopEntityModel
        {
            [NopResourceDisplayName("RewardPoints.Fields.Points")]
            public int Points { get; set; }

            [NopResourceDisplayName("RewardPoints.Fields.PointsBalance")]
            public int PointsBalance { get; set; }

            [NopResourceDisplayName("RewardPoints.Fields.Message")]
            public string Message { get; set; }

            [NopResourceDisplayName("RewardPoints.Fields.Date")]
            public string CreatedOnStr { get; set; }
        }

        #endregion
    }
}