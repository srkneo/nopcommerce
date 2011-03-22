﻿using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Localization;
using Nop.Services.Security.Permissions;
using Nop.Web.Framework.Controllers;
using Nop.Web.MVC.Areas.Admin.Models;
using Nop.Web.MVC.Models;

namespace Nop.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILanguageService _languageService;
        private IWorkContext _workContext;

        public HomeController(ILanguageService languageService, IWorkContext workContext)
        {
            _workContext = workContext;
            _languageService = languageService;
        }

        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult LanguageSelector()
        {
            var model = new LanguageSelectorModel();
            var avaibleLanguages = _languageService.GetAllLanguages();
            model.CurrentLanguage = AutoMapper.Mapper.Map<Language, LanguageModel>(_workContext.WorkingLanguage);
            model.AvaibleLanguages = avaibleLanguages.Select(AutoMapper.Mapper.Map<Language, LanguageModel>).ToList();
            return PartialView(model);
        }

        public ActionResult LanguageSelected(int id)
        {
            var language = _languageService.GetLanguageById(id);
            if(language != null)
            {
                _workContext.WorkingLanguage = language;
            }
            var model = new LanguageSelectorModel();
            var avaibleLanguages = _languageService.GetAllLanguages();
            model.CurrentLanguage = AutoMapper.Mapper.Map<Language, LanguageModel>(_workContext.WorkingLanguage);
            model.AvaibleLanguages = avaibleLanguages.Select(AutoMapper.Mapper.Map<Language, LanguageModel>).ToList();
            model.IsAjaxRequest = true;
            return PartialView("LanguageSelector", model);
        }
    }
}