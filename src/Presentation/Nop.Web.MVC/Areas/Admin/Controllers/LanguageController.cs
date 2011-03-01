﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core.Domain.Localization;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework.Controllers;
using Nop.Web.MVC.Areas.Admin.Models;
using Telerik.Web.Mvc;

namespace Nop.Web.MVC.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class LanguageController : Controller
    {
        private ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }


        public ActionResult Index()
        {
            return View("List");
        }

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
                Data = languages.Select(x => new LanguageModel(x)),
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
                Data = languages.Select(x => new LanguageModel(x)),
                Total = languages.Count()
            };

            //var categories = _languageService.GetAllCategories(command.Page - 1, command.PageSize);
            //model.Data = categories.Select(x =>
            //    new { Id = Url.Action("Edit", new { x.Id }), x.Name, x.DisplayOrder, Breadcrumb = GetCategoryBreadCrumb(x), x.Published });
            //model.Total = categories.TotalCount;
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
            return View(new LanguageModel(language));
        }

        [HttpPost]
        public ActionResult Edit(LanguageModel languageModel)
        {
            var language = _languageService.GetLanguageById(languageModel.Id);
            UpdateInstance(language, languageModel);
            _languageService.UpdateLanguage(language);
            return Edit(language.Id);
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            var language = _languageService.GetLanguageById(id);
            if (language != null)
            {
                _languageService.DeleteLanguage(language);
            }
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
            var language = new Language();
            UpdateInstance(language, model);
            _languageService.InsertLanguage(language);
            return RedirectToAction("Edit", new { id = language.Id });
        }

        #endregion

        #endregion

        #region Resources

        #endregion

        public void UpdateInstance(Language language, LanguageModel model)
        {
            language.Id = model.Id;
            language.Name = model.Name;
            language.LanguageCulture = model.LanguageCulture;
            language.FlagImageFileName = model.FlagImageFileName;
            language.Published = model.Published;
            language.DisplayOrder = model.DisplayOrder;
        }
    }
}