﻿using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Admin.Models.Directory;
using Nop.Admin.Validators;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Shipping
{
    public class ShippingMethodRestrictionModel : BaseNopModel
    {
        public ShippingMethodRestrictionModel()
        {
            AvailableShippingMethods = new List<ShippingMethodModel>();
            AvailableCountries = new List<CountryModel>();
            Restricted = new Dictionary<int, IDictionary<int, bool>>();
        }
        public IList<ShippingMethodModel> AvailableShippingMethods { get; set; }
        public IList<CountryModel> AvailableCountries { get; set; }

        //[country id] / [shipping method id] / [restricted]
        public IDictionary<int, IDictionary<int, bool>> Restricted { get; set; }
    }
}