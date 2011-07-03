﻿using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Models.Catalog;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Web.Framework.Controllers;
using Telerik.Web.Mvc;
using Nop.Services.Security;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public class SpecificationAttributeController : BaseNopController
    {
        #region Fields

        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;

        #endregion Fields

        #region Constructors

        public SpecificationAttributeController(ISpecificationAttributeService specificationAttributeService,
            ILanguageService languageService, ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService, ICustomerActivityService customerActivityService,
            IPermissionService permissionService)
        {
            this._specificationAttributeService = specificationAttributeService;
            this._languageService = languageService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
        }

        #endregion
        
        #region Utilities

        [NonAction]
        public void UpdateAttributeLocales(SpecificationAttribute specificationAttribute, SpecificationAttributeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(specificationAttribute,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);
            }
        }

        [NonAction]
        public void UpdateOptionLocales(SpecificationAttributeOption specificationAttributeOption, SpecificationAttributeOptionModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(specificationAttributeOption,
                                                               x => x.Name,
                                                               localized.Name,
                                                               localized.LanguageId);
            }
        }

        #endregion
        
        #region Specification attributes

        //list
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var specificationAttributes = _specificationAttributeService.GetSpecificationAttributes();
            var gridModel = new GridModel<SpecificationAttributeModel>
            {
                Data = specificationAttributes.Select(x => x.ToModel()),
                Total = specificationAttributes.Count()
            };
            return View(gridModel);
        }

        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult List(GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var specificationAttributes = _specificationAttributeService.GetSpecificationAttributes();
            var gridModel = new GridModel<SpecificationAttributeModel>
            {
                Data = specificationAttributes.Select(x => x.ToModel()),
                Total = specificationAttributes.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        
        //create
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var model = new SpecificationAttributeModel();
            //locales
            AddLocales(_languageService, model.Locales);
            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Create(SpecificationAttributeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var specificationAttribute = model.ToEntity();
                _specificationAttributeService.InsertSpecificationAttribute(specificationAttribute);
                UpdateAttributeLocales(specificationAttribute, model);

                //activity log
                _customerActivityService.InsertActivity("AddNewSpecAttribute", _localizationService.GetResource("ActivityLog.AddNewSpecAttribute"), specificationAttribute.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.SpecificationAttributes.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = specificationAttribute.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //edit
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var specificationAttribute = _specificationAttributeService.GetSpecificationAttributeById(id);
            if (specificationAttribute == null)
                throw new ArgumentException("No specification attribute found with the specified id", "id");
            var model = specificationAttribute.ToModel();
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = specificationAttribute.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost, FormValueExists("save", "save-continue", "continueEditing")]
        public ActionResult Edit(SpecificationAttributeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var specificationAttribute = _specificationAttributeService.GetSpecificationAttributeById(model.Id);
            if (specificationAttribute == null)
                throw new ArgumentException("No specification attribute found with the specified id");
            if (ModelState.IsValid)
            {
                specificationAttribute = model.ToEntity(specificationAttribute);
                _specificationAttributeService.UpdateSpecificationAttribute(specificationAttribute);

                UpdateAttributeLocales(specificationAttribute, model);

                //activity log
                _customerActivityService.InsertActivity("EditSpecAttribute", _localizationService.GetResource("ActivityLog.EditSpecAttribute"), specificationAttribute.Name);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.SpecificationAttributes.Updated"));
                return continueEditing ? RedirectToAction("Edit", specificationAttribute.Id) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //delete
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var specificationAttribute = _specificationAttributeService.GetSpecificationAttributeById(id);
            _specificationAttributeService.DeleteSpecificationAttribute(specificationAttribute);

            //activity log
            _customerActivityService.InsertActivity("DeleteSpecAttribute", _localizationService.GetResource("ActivityLog.DeleteSpecAttribute"), specificationAttribute.Name);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Attributes.SpecificationAttributes.Deleted"));
            return RedirectToAction("List");
        }

        #endregion

        #region Specification attribute options

        //list
        [HttpPost, GridAction(EnableCustomBinding = true)]
        public ActionResult OptionList(int specificationAttributeId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var options = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute(specificationAttributeId);
            var gridModel = new GridModel<SpecificationAttributeOptionModel>
            {
                Data = options.Select(x => 
                    {
                        var model = x.ToModel();
                        //locales
                        //AddLocales(_languageService, model.Locales, (locale, languageId) =>
                        //{
                        //    locale.Name = x.GetLocalized(y => y.Name, languageId, false, false);
                        //});
                        return model;
                    }),
                Total = options.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }

        //create
        public ActionResult OptionCreatePopup(int specificationAttributeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var model = new SpecificationAttributeOptionModel();
            model.SpecificationAttributeId = specificationAttributeId;
            //locales
            AddLocales(_languageService, model.Locales);
            return View(model);
        }

        [HttpPost]
        public ActionResult OptionCreatePopup(string btnId, SpecificationAttributeOptionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var specificationAttribute = _specificationAttributeService.GetSpecificationAttributeById(model.SpecificationAttributeId);
            if (specificationAttribute == null)
                throw new ArgumentException("No specification attribute found with the specified id");

            if (ModelState.IsValid)
            {
                var sao = model.ToEntity();

                _specificationAttributeService.InsertSpecificationAttributeOption(sao);
                UpdateOptionLocales(sao, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //edit
        public ActionResult OptionEditPopup(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var sao = _specificationAttributeService.GetSpecificationAttributeOptionById(id);
            if (sao == null)
                throw new ArgumentException("No specification attribute option found with the specified id", "id");
            var model = sao.ToModel();
            //locales
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = sao.GetLocalized(x => x.Name, languageId, false, false);
            });

            return View(model);
        }

        [HttpPost]
        public ActionResult OptionEditPopup(string btnId, SpecificationAttributeOptionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var sao = _specificationAttributeService.GetSpecificationAttributeOptionById(model.Id);
            if (sao == null)
                throw new ArgumentException("No specification attribute option found with the specified id");
            if (ModelState.IsValid)
            {
                sao = model.ToEntity(sao);
                _specificationAttributeService.UpdateSpecificationAttributeOption(sao);

                UpdateOptionLocales(sao, model);

                ViewBag.RefreshPage = true;
                ViewBag.btnId = btnId;
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        //delete
        [GridAction(EnableCustomBinding = true)]
        public ActionResult OptionDelete(int optionId, int specificationAttributeId, GridCommand command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            var sao = _specificationAttributeService.GetSpecificationAttributeOptionById(optionId);
            _specificationAttributeService.DeleteSpecificationAttributeOption(sao);

            return OptionList(specificationAttributeId, command);
        }


        //ajax
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetOptionsByAttributeId(string attributeId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCatalog))
                return AccessDeniedView();

            // This action method gets called via an ajax request
            if (String.IsNullOrEmpty(attributeId))
                throw new ArgumentNullException("attributeId");

            var options = _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttribute(Convert.ToInt32(attributeId));
            var result = (from o in options
                          select new { id = o.Id, name = o.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
