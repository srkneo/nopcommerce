﻿using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Nop.Admin.Models;
using Nop.Admin.Models.Messages;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;

using Telerik.Web.Mvc;
using System.Collections.Generic;

namespace Nop.Admin.Controllers
{
	[AdminAuthorize]
	public class CampaignController : BaseNopController
	{
        private readonly ICampaignService _campaignService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly ILocalizationService _localizationService;
        private readonly IMessageTokenProvider _messageTokenProvider;

        public CampaignController(ICampaignService campaignService,
            IDateTimeHelper dateTimeHelper, IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ILocalizationService localizationService, IMessageTokenProvider messageTokenProvider)
		{
            this._campaignService = campaignService;
            this._dateTimeHelper = dateTimeHelper;
            this._emailAccountService = emailAccountService;
            this._emailAccountSettings = emailAccountSettings;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._localizationService = localizationService;
            this._messageTokenProvider = messageTokenProvider;
		}

        private string FormatTokens(string[] tokens)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                sb.Append(token);
                if (i != tokens.Length - 1)
                    sb.Append(", ");
            }

            return sb.ToString();
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

		public ActionResult List()
        {
            var campaigns = _campaignService.GetAllCampaigns();
            var gridModel = new GridModel<CampaignModel>
            {
                Data = campaigns.Select(x =>
                {
                    var model = x.ToModel();
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    return model;
                }),
                Total = campaigns.Count
            };
            return View(gridModel);
		}

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            var campaigns = _campaignService.GetAllCampaigns();
            var gridModel = new GridModel<CampaignModel>
            {
                Data = campaigns.Select(x =>
                {
                    var model = x.ToModel();
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc);
                    return model;
                }),
                Total = campaigns.Count
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        public ActionResult Create()
        {
            var model = new CampaignModel();
            model.AllowedTokens = FormatTokens(_messageTokenProvider.GetListOfCampaignAllowedTokens());
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Create(CampaignModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var campaign = model.ToEntity();
                campaign.CreatedOnUtc = DateTime.UtcNow;
                _campaignService.InsertCampaign(campaign);

                SuccessNotification(_localizationService.GetResource("Admin.Promotions.Campaigns.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = campaign.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            model.AllowedTokens = FormatTokens(_messageTokenProvider.GetListOfCampaignAllowedTokens());
            return View(model);
        }

		public ActionResult Edit(int id)
		{
            var campaign = _campaignService.GetCampaignById(id);
            if (campaign == null)
                throw new ArgumentException("No campaign found with the specified id", "id");

            var model = campaign.ToModel();
            model.AllowedTokens = FormatTokens(_messageTokenProvider.GetListOfCampaignAllowedTokens());
            return View(model);
		}

        [HttpPost]
        [FormValueExists("save", "save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(CampaignModel model, bool continueEditing)
        {
            var campaign = _campaignService.GetCampaignById(model.Id);
            if (campaign == null)
                throw new ArgumentException("No campaign found with the specified id");

            if (ModelState.IsValid)
            {
                campaign = model.ToEntity(campaign);
                _campaignService.UpdateCampaign(campaign);

                SuccessNotification(_localizationService.GetResource("Admin.Promotions.Campaigns.Updated"));
                return continueEditing ? RedirectToAction("Edit", new { id = campaign.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            model.AllowedTokens = FormatTokens(_messageTokenProvider.GetListOfCampaignAllowedTokens());
            return View(model);
		}

        [HttpPost,ActionName("Edit")]
        [FormValueRequired("send-test-email")]
        public ActionResult SendTestEmail(CampaignModel model)
        {
            var campaign = _campaignService.GetCampaignById(model.Id);
            if (campaign == null)
                throw new ArgumentException("No campaign found with the specified id");


            model.AllowedTokens = FormatTokens(_messageTokenProvider.GetListOfCampaignAllowedTokens());

            try
            {
                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    throw new NopException("Email account could not be loaded");
                _campaignService.SendCampaign(campaign, emailAccount, model.TestEmail);
                SuccessNotification(_localizationService.GetResource("Admin.Promotions.Campaigns.TestEmailSentToCustomers"), false);
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc, false);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("send-mass-email")]
        public ActionResult SendMassEmail(CampaignModel model)
        {
            var campaign = _campaignService.GetCampaignById(model.Id);
            if (campaign == null)
                throw new ArgumentException("No campaign found with the specified id");


            model.AllowedTokens = FormatTokens(_messageTokenProvider.GetListOfCampaignAllowedTokens());

            try
            {
                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    throw new NopException("Email account could not be loaded");

                var subscriptions = _newsLetterSubscriptionService.GetAllNewsLetterSubscriptions(null, false);
                var totalEmailsSent = _campaignService.SendCampaign(campaign, emailAccount, subscriptions);
                SuccessNotification(string.Format(_localizationService.GetResource("Admin.Promotions.Campaigns.MassEmailSentToCustomers"), totalEmailsSent), false);
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc, false);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
        {
            var campaign = _campaignService.GetCampaignById(id);
            if (campaign == null)
                throw new ArgumentException("No campaign found with the specified id", "id");
            _campaignService.DeleteCampaign(campaign);

            SuccessNotification(_localizationService.GetResource("Admin.Promotions.Campaigns.Deleted"));
			return RedirectToAction("List");
		}
	}
}
