﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Nop.Admin.Models;
using Nop.Admin.Models.Common;
using Nop.Admin.Models.Directory;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Tax;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Services.Tax;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class CommonController : BaseNopController
    {
        #region Fields

        private readonly IPaymentService _paymentService;
        private readonly IShippingService _shippingService;
        private readonly ICurrencyService _currencyService;
        private readonly IMeasureService _measureService;
        private readonly ICustomerService _customerService;
        private readonly IWebHelper _webHelper;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly CurrencySettings _currencySettings;
        private readonly MeasureSettings _measureSettings;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Constructors

        public CommonController(IPaymentService paymentService, IShippingService shippingService,
            ICurrencyService currencyService, IMeasureService measureService,
            ICustomerService customerService, IWebHelper webHelper,
            StoreInformationSettings storeInformationSettings, CurrencySettings currencySettings,
            MeasureSettings measureSettings, IDateTimeHelper dateTimeHelper)
        {
            this._paymentService = paymentService;
            this._shippingService = shippingService;
            this._currencyService = currencyService;
            this._measureService = measureService;
            this._customerService = customerService;
            this._webHelper = webHelper;
            this._storeInformationSettings = storeInformationSettings;
            this._currencySettings = currencySettings;
            this._measureSettings = measureSettings;
            this._dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        public ActionResult SystemInfo()
        {
            var model = new SystemInfoModel();
            model.NopVersion = _storeInformationSettings.CurrentVersion;
            try
            {
                model.OperatingSystem = Environment.OSVersion.VersionString;
            }
            catch (Exception) { }
            try
            {
                model.AspNetInfo = RuntimeEnvironment.GetSystemVersion();
            }
            catch (Exception) { }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch (Exception) { }
            model.ServerTimeZone = TimeZone.CurrentTimeZone.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.UtcTime = DateTime.UtcNow;
            //Environment.GetEnvironmentVariable("USERNAME");
            return View(model);
        }

        public ActionResult Warnings()
        {
            var model = new List<SystemWarningModel>();

            //store URL
            if (!String.IsNullOrEmpty(_storeInformationSettings.StoreUrl) &&
                _storeInformationSettings.StoreUrl.Equals(_webHelper.GetStoreLocation(false), StringComparison.InvariantCultureIgnoreCase))
                model.Add(new SystemWarningModel()
                    {
                        Level = SystemWarningLevel.Pass,
                        Text = "Specified store URL matches this store URL",
                    });
            else
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Warning,
                    Text = string.Format("Specified store URL ({0}) doesn't match this store URL ({1})", _storeInformationSettings.StoreUrl, _webHelper.GetStoreLocation(false))
                });


            //primary exchange rate currency
            var perCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryExchangeRateCurrencyId);
            if (perCurrency != null)
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Pass,
                    Text = "Primary exchange rate currency is set",
                });
            else
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Fail,
                    Text = "Primary exchange rate currency is not set"
                });


            //primary store currency
            var pscCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
            if (pscCurrency != null)
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Pass,
                    Text = "Primary store currency is set",
                });
            else
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Fail,
                    Text = "Primary store currency is not set"
                });


            //base measure weight
            var bWeight = _measureService.GetMeasureWeightById(_measureSettings.BaseWeightId);
            if (bWeight != null)
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Pass,
                    Text = "Default weight is set",
                });
            else
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Fail,
                    Text = "Default weight is not set"
                });


            //base dimension weight
            var bDimension = _measureService.GetMeasureDimensionById(_measureSettings.BaseDimensionId);
            if (bDimension != null)
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Pass,
                    Text = "Default dimension is set",
                });
            else
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Fail,
                    Text = "Default dimension is not set"
                });

            //shipping rate coputation methods
            if (_shippingService.LoadActiveShippingRateComputationMethods()
                .Where(x => x.ShippingRateComputationMethodType == ShippingRateComputationMethodType.Offline)
                .Count() > 1)
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Warning,
                    Text = "Only one offline shipping rate computation method is recommended to use"
                });

            //payment methods
            if (_paymentService.LoadActivePaymentMethods()
                .Count() > 0)
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Pass,
                    Text = "Payment methods are OK"
                });
            else
                model.Add(new SystemWarningModel()
                {
                    Level = SystemWarningLevel.Fail,
                    Text = "You don't have active payment methods"
                });

            //TODO check customers which have at least one associated 'User' record and are not in 'Registered' customer role
            //Also check users which don't have associated 'Customer' record

            return View(model);
        }

        public ActionResult Maintenance()
        {
            var model = new MaintenanceModel();
            model.DeleteGuests.EndDate = DateTime.UtcNow.AddDays(-7);
            model.DeleteGuests.OnlyWithoutShoppingCart = true;
            return View(model);
        }

        [HttpPost, ActionName("Maintenance")]
        [FormValueRequired("delete-guests")]
        public ActionResult MaintenanceDeleteGuests(MaintenanceModel model)
        {
            DateTime? startDateValue = (model.DeleteGuests.StartDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DeleteGuests.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.DeleteGuests.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DeleteGuests.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            model.DeleteGuests.NumberOfDeletedCustomers = _customerService.DeleteGuestCustomers(startDateValue, endDateValue, model.DeleteGuests.OnlyWithoutShoppingCart);

            return View(model);
        }

        [HttpPost, ActionName("Maintenance")]
        [FormValueRequired("delete-exported-files")]
        public ActionResult MaintenanceDeleteFiles(MaintenanceModel model)
        {
            DateTime? startDateValue = (model.DeleteExportedFiles.StartDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DeleteExportedFiles.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.DeleteExportedFiles.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DeleteExportedFiles.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);


            model.DeleteExportedFiles.NumberOfDeletedFiles = 0;
            string path = string.Format("{0}content\\files\\exportimport\\", this.Request.PhysicalApplicationPath);
            foreach (var fullPath in System.IO.Directory.GetFiles(path))
            {
                try
                {
                    var fileName = Path.GetFileName(fullPath);
                    if (fileName.Equals("index.htm", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var info = new FileInfo(fullPath);
                    if ((!startDateValue.HasValue || startDateValue.Value < info.CreationTimeUtc)&&
                        (!endDateValue.HasValue || info.CreationTimeUtc < endDateValue.Value))
                    {
                        System.IO.File.Delete(fullPath);
                        model.DeleteExportedFiles.NumberOfDeletedFiles++;
                    }
                }
                catch (Exception exc)
                {
                    //TODO log exception
                }
            }

            return View(model);
        }
        
        #endregion
    }
}