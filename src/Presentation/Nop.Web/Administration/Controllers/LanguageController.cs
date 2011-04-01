﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Admin.Models;
using Nop.Core.Domain.Localization;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using Nop.Admin.Models;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Nop.Web.Framework;
using Telerik.Web.Mvc.Extensions;

namespace Nop.Admin.Controllers
{
	[AdminAuthorize]
    public class LanguageController : BaseNopController
	{
		#region Fields

		private readonly ILanguageService _languageService;
		private ILocalizationService _localizationService;

		#endregion Fields 

		#region Constructors

		public LanguageController(ILanguageService languageService, ILocalizationService localizationService)
		{
			_localizationService = localizationService;
			_languageService = languageService;
		}

		#endregion Constructors 

		#region Methods

		public ActionResult Index()
		{
			return View("List");
		}

        //public void UpdateInstance(Language language, LanguageModel model)
        //{
        //    language.Id = model.Id;
        //    language.Name = model.Name;
        //    language.LanguageCulture = model.LanguageCulture;
        //    language.FlagImageFileName = model.FlagImageFileName;
        //    language.Published = model.Published;
        //    language.DisplayOrder = model.DisplayOrder;
        //}

        //public void UpdateInstance(LocaleStringResource resource, LanguageResourceModel model)
        //{
        //    resource.ResourceName = model.Name;
        //    resource.ResourceValue = model.Value;
        //}

		#endregion Methods 
        
		#region Languages

		#region List

		public ActionResult List()
		{
			//if (!_permissionService.Authorize(CatalogPermissionProvider.ManageCategories))
			//{
			//TODO redirect to access denied page
			//}

			var languages = _languageService.GetAllLanguages(true);
			var gridModel = new GridModel<LanguageModel>
			{
				Data = languages.Select(x => x.ToModel()),
				Total = languages.Count()
			};
			return View(gridModel);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult List(GridCommand command)
		{
			var languages = _languageService.GetAllLanguages(true);
			var gridModel = new GridModel<LanguageModel>
			{
				Data = languages.Select(x => x.ToModel()),
				Total = languages.Count()
			};
			return new JsonResult
			{
				Data = gridModel
			};
		}

		#endregion

		#region Edit

		public ActionResult Edit(int id)
		{
			var language = _languageService.GetLanguageById(id);
			if (language == null) throw new ArgumentException("No language found with the specified id", "id");
			return View(language.ToModel());
		}

		[HttpPost]
		public ActionResult Edit(LanguageModel languageModel)
		{
            if (!ModelState.IsValid)
            {
                return View(languageModel);
            }
			var language = _languageService.GetLanguageById(languageModel.Id);
		    language = languageModel.ToEntity(language);
			_languageService.UpdateLanguage(language);
			return Edit(language.Id);
		}

		#endregion

		#region Delete

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			var language = _languageService.GetLanguageById(id);
			_languageService.DeleteLanguage(language);
			return RedirectToAction("List");
		}

		#endregion

		#region Create

		public ActionResult Create()
		{
			return View(new LanguageModel());
		}

		[HttpPost]
		public ActionResult Create(LanguageModel model)
		{
		    var language = model.ToEntity();
			_languageService.InsertLanguage(language);
			return RedirectToAction("Edit", new { id = language.Id });
		}

		#endregion

		#endregion

		#region Resources

		#region List

		public ActionResult Resources(int languageId)
		{
			//if (!_permissionService.Authorize(CatalogPermissionProvider.ManageCategories))
			//{
			//TODO redirect to access denied page
			//}
			ViewBag.AllLanguages = _languageService.GetAllLanguages(true).Select(x => new DropDownItem
																						  {
																							  Selected = (x.Id.Equals(languageId)),
																							  Text = x.Name,
																							  Value = x.Id.ToString()
																						  }).ToList();
		    var language = _languageService.GetLanguageById(languageId);
		    ViewBag.LanguageId = languageId;
		    ViewBag.LanguageName = language.Name;

			var resources = _localizationService.GetAllResourcesByLanguageId(languageId);
			var gridModel = new GridModel<LanguageResourceModel>
			{
				Data = resources.Take(20).Select(x => x.Value.ToModel()),
				Total = resources.Count
			};
			return View(gridModel);
		}

		[HttpPost, GridAction(EnableCustomBinding = true)]
		public ActionResult Resources(int languageId, GridCommand command)
		{
		    var resources = _localizationService.GetAllResourcesByLanguageId(languageId).Select(x => x.Value)
		        .Select(x => x.ToModel())
		        .ForCommand(command);

            var model = new GridModel<LanguageResourceModel>
                            {
                                Data = resources.PagedForCommand(command),
                                Total = resources.Count()
                            };
		    return new JsonResult
			{
				Data = model
			};
		}

		#endregion

        #region Edit

        [GridAction(EnableCustomBinding=true)]
        public ActionResult ResourceUpdate(LanguageResourceModel model, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Resources", new {model.LanguageId});
            }

            var resource = _localizationService.GetLocaleStringResourceById(model.Id);
            resource = model.ToEntity(resource);
            _localizationService.UpdateLocaleStringResource(resource);

            #region Return a model with the current page and pagesize

            var resources = _localizationService.GetAllResourcesByLanguageId(model.LanguageId).Select(x => x.Value)
                .Select(x => x.ToModel())
                .ForCommand(command);
            var gridModel = new GridModel<LanguageResourceModel>
                                {
                                    Data = resources.PagedForCommand(command),
                                    Total = resources.Count()
                                };
            return new JsonResult
            {
                Data = gridModel
            };

            #endregion
        }

        #endregion

        #region Add

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ResourceAdd(int id, LanguageResourceModel resourceModel, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                //TODO:Find out how telerik handles errors
                return new JsonResult {Data = "error"};
            }

            var resource = new LocaleStringResource {LanguageId = id};
            resource = resourceModel.ToEntity(resource);
            _localizationService.InsertLocaleStringResource(resource);

            var resources = _localizationService.GetAllResourcesByLanguageId(id).Select(x => x.Value)
                .Select(x => x.ToModel())
                .ForCommand(command);

            var gridModel = new GridModel<LanguageResourceModel>
            {
                Data = resources.PagedForCommand(command),
                Total = resources.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion

        #region Delete

        [GridAction(EnableCustomBinding = true)]
        public ActionResult ResourceDelete(int id, int languageId, GridCommand command)
        {
            if (!ModelState.IsValid)
            {
                //TODO:Find out how telerik handles errors
                return new JsonResult { Data = "error" };
            }

            var resource = _localizationService.GetLocaleStringResourceById(id);
            _localizationService.DeleteLocaleStringResource(resource);

            var resources = _localizationService.GetAllResourcesByLanguageId(languageId).Select(x => x.Value)
                .Select(x => x.ToModel())
                .ForCommand(command);

            var gridModel = new GridModel<LanguageResourceModel>
            {
                Data = resources.PagedForCommand(command),
                Total = resources.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        #endregion

        #endregion
    }
}
