﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Models;
using Nop.Admin.Models.Logging;
using Nop.Admin.Models.Orders;
using Nop.Core;
using Nop.Services.Customers;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using Nop.Core.Domain.Logging;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class LogController : BaseNopController
    {
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;

        public LogController(ILogger logger, IWorkContext workContext,
            ILocalizationService localizationService, IDateTimeHelper dateTimeHelper)
        {
            this._logger = logger;
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new LogListModel();
            model.AvailableLogLevels = LogLevel.Debug.ToSelectList(false).ToList();
            model.AvailableLogLevels.Insert(0, new SelectListItem() { Text = _localizationService.GetResource("Admin.Common.All"), Value = "0" });

            return View(model);
        }

        [GridAction(EnableCustomBinding = true)]
        public ActionResult LogList(GridCommand command, LogListModel model)
        {
            DateTime? createdOnFromValue = (model.CreatedOnFrom == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnFrom.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? createdToFromValue = (model.CreatedOnTo == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.CreatedOnTo.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            LogLevel? logLevel = model.LogLevelId > 0 ? (LogLevel?)(model.LogLevelId) : null;


            var orders = _logger.GetAllLogs(createdOnFromValue, createdToFromValue, model.Message,
                logLevel, command.Page - 1, command.PageSize);
            var gridModel = new GridModel<LogModel>
            {
                Data = orders.Select(x =>
                {
                    return new LogModel()
                    {
                        Id = x.Id,
                        LogLevel = x.LogLevel.GetLocalizedEnum(_localizationService, _workContext),
                        ShortMessage = x.ShortMessage,
                        FullMessage = x.FullMessage,
                        IpAddress = x.IpAddress,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer!= null ? x.Customer.GetFullName() : null,
                        PageUrl = x.PageUrl,
                        ReferrerUrl = x.ReferrerUrl,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                    };
                }),
                Total = orders.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        
        [HttpPost, ActionName("List")]
        [FormValueRequired("clearall")]
        public ActionResult ClearAll()
        {
            _logger.ClearLog();

            SuccessNotification(_localizationService.GetResource("Admin.System.Log.Cleared"));
            return RedirectToAction("List");
        }

        public ActionResult View(int id)
        {
            var log = _logger.GetLogById(id);
            if (log == null)
                throw new ArgumentException("No log found with the specified id", "id");

            var model = new LogModel()
            {
                Id = log.Id,
                LogLevel = log.LogLevel.GetLocalizedEnum(_localizationService, _workContext),
                ShortMessage = log.ShortMessage,
                FullMessage = log.FullMessage,
                IpAddress = log.IpAddress,
                CustomerId = log.CustomerId,
                CustomerName = log.Customer != null ? log.Customer.GetFullName() : null,
                PageUrl = log.PageUrl,
                ReferrerUrl = log.ReferrerUrl,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(log.CreatedOnUtc, DateTimeKind.Utc)
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var log = _logger.GetLogById(id);
            if (log == null)
                throw new ArgumentException("No log found with the specified id", "id");

            _logger.DeleteLog(log);


            SuccessNotification(_localizationService.GetResource("Admin.System.Log.Deleted"));
            return RedirectToAction("List");
        }

    }
}
