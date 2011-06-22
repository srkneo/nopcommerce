﻿using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Models.Plugins;
using Nop.Core.Plugins;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace Nop.Admin.Controllers
{
	[AdminAuthorize]
    public class PluginController : BaseNopController
	{
		#region Fields

        private readonly IPluginFinder _pluginFinder;
        private readonly ILocalizationService _localizationService;

	    #endregion

		#region Constructors

        public PluginController(IPluginFinder pluginFinder,
            ILocalizationService localizationService)
		{
            this._pluginFinder = pluginFinder;
            this._localizationService = localizationService;
		}

		#endregion 

        #region Methods

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var pluginDescriptors = _pluginFinder.GetPluginDescriptors();
            var model = new GridModel<PluginModel>
            {
                Data = pluginDescriptors.Select(x => x.ToModel())
                .OrderBy(x => x.Group)
                .ThenBy(x => x.DisplayOrder),
                Total = pluginDescriptors.Count()
            };
            return View(model);
        }

        public ActionResult Install(string systemName)
        {
            try
            {
                var pluginDescriptor = _pluginFinder.GetPluginDescriptors()
                    .Where(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
                if (pluginDescriptor == null)
                    throw new ArgumentException("No plugin found with the specified system name");

                //check whether plugin is not installed
                if (pluginDescriptor.Installed)
                    throw new ArgumentException("Plugin is already installed");

                //install plugin
                pluginDescriptor.Instance().Install();
                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Plugins.Installed"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }
             
            return RedirectToAction("List");
        }

        public ActionResult Uninstall(string systemName)
        {
            try
            {
                var pluginDescriptor = _pluginFinder.GetPluginDescriptors()
                    .Where(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
                if (pluginDescriptor == null)
                    throw new ArgumentException("No plugin found with the specified system name");

                //check whether plugin is installed
                if (!pluginDescriptor.Installed)
                    throw new ArgumentException("Plugin is not installed");

                //uninstall plugin
                pluginDescriptor.Instance().Uninstall();
                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Plugins.Uninstalled"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return RedirectToAction("List");
        }
        #endregion
    }
}
