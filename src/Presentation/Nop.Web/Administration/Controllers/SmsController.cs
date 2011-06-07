﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Admin.Models;
using Nop.Admin.Models.Messages;
using Nop.Core.Domain.Messages;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace Nop.Admin.Controllers
{
	[AdminAuthorize]
    public class SmsController : BaseNopController
	{
		#region Fields

        private readonly ISmsService _smsService;
        private SmsSettings _smsSettings;
        private readonly ISettingService _settingService;

		#endregion

		#region Constructors

        public SmsController(ISmsService smsService, SmsSettings smsSettings,
            ISettingService settingService)
		{
            this._smsService = smsService;
            this._smsSettings = smsSettings;
            this._settingService = settingService;
		}

		#endregion Constructors 

        #region Methods

        public ActionResult Providers()
        {
            var smsProvidersModel = new List<SmsProviderModel>();
            var smsProviders = _smsService.LoadAllSmsProviders();
            foreach (var smsProvider in smsProviders)
            {
                var tmp1 = smsProvider.ToModel();
                tmp1.IsActive = smsProvider.IsSmsProviderActive(_smsSettings);
                smsProvidersModel.Add(tmp1);
            }
            var gridModel = new GridModel<SmsProviderModel>
            {
                Data = smsProvidersModel,
                Total = smsProvidersModel.Count()
            };
            return View(gridModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult Providers(GridCommand command)
        {
            var smsProvidersModel = new List<SmsProviderModel>();
            var smsProviders = _smsService.LoadAllSmsProviders();
            foreach (var smsProvider in smsProviders)
            {
                var tmp1 = smsProvider.ToModel();
                tmp1.IsActive = smsProvider.IsSmsProviderActive(_smsSettings);
                smsProvidersModel.Add(tmp1);
            }
            smsProvidersModel = smsProvidersModel.ForCommand(command).ToList();
            var gridModel = new GridModel<SmsProviderModel>
            {
                Data = smsProvidersModel,
                Total = smsProvidersModel.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ProviderUpdate(SmsProviderModel model, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Methods");
            }

            //TODO an issue: when a store owner clicks on 'Update' button and then 'Cancel' button,
            //the 'Configure method' hyperlink disappears
            
            var smsProvider = _smsService.LoadSmsProviderBySystemName(model.SystemName);
            if (smsProvider.IsSmsProviderActive(_smsSettings))
            {
                if (!model.IsActive)
                {
                    //mark as disabled
                    _smsSettings.ActiveSmsProviderSystemNames.Remove(smsProvider.SystemName);
                    _settingService.SaveSetting(_smsSettings);
                }
            }
            else
            {
                if (model.IsActive)
                {
                    //mark as active
                    _smsSettings.ActiveSmsProviderSystemNames.Add(smsProvider.SystemName);
                    _settingService.SaveSetting(_smsSettings);
                }
            }

            return Providers(command);
        }

        public ActionResult ConfigureProvider(string systemName)
        {
            var smsProvider = _smsService.LoadSmsProviderBySystemName(systemName);
            if (smsProvider == null) throw new ArgumentException("No SMS provider found with the specified system name", "systemName");

            var model = smsProvider.ToModel();
            string actionName, controllerName;
            RouteValueDictionary routeValues;
            smsProvider.GetConfigurationRoute(out actionName, out controllerName, out routeValues);
            model.ConfigurationActionName = actionName;
            model.ConfigurationControllerName = controllerName;
            model.ConfigurationRouteValues = routeValues;
            return View(model);
        }

        #endregion
    }
}