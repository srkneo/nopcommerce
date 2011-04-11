﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Admin.Models;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Tax;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Tax;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace Nop.Admin.Controllers
{
	[AdminAuthorize]
    public class CountryController : BaseNopController
	{
		#region Fields

        private readonly ICountryService _countryService;
        private IStateProvinceService _stateProvinceService;

	    #endregion Fields 

		#region Constructors

        public CountryController(ICountryService countryService, 
            IStateProvinceService stateProvinceService)
		{
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
		}

		#endregion Constructors 

        #region States / provinces

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStatesByCountryId(string countryId, bool addEmptyStateIfRequired)
        {
            // This action method gets called via an ajax request
            if (String.IsNullOrEmpty(countryId))
                throw new ArgumentNullException("countryId");

            var states = new List<StateProvince>();
            var country = _countryService.GetCountryById(Convert.ToInt32(countryId));
            if (country != null)
                states = country.StateProvinces.ToList();
            var result = (from s in states
                         select new { id = s.Id, name = s.Name }).ToList();
            if (addEmptyStateIfRequired && result.Count == 0)
                result.Insert(0, new { id = 0, name = "Other (Non US)" });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}