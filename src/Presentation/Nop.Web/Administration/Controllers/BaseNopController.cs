﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Services.Localization;
using Nop.Web.Framework.Localization;

namespace Nop.Admin.Controllers
{
    public class BaseNopController : Controller
    {
        public void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            AddLocales(languageService, locales, null);
        }

        public void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales, Action<TLocalizedModelLocal, int> configure) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            foreach (var language in languageService.GetAllLanguages(true))
            {
                var locale = Activator.CreateInstance<TLocalizedModelLocal>();
                locale.Language = language;
                if (configure != null)
                {
                    configure.Invoke(locale, locale.Language.Id);
                }
                locales.Add(locale);
            }
        }
    }
}