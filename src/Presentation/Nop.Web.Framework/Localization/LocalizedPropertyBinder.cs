﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;

namespace Nop.Web.Framework.Localization
{
    public class LocalizedPropertyBinder : IModelBinder
    {

        private ILanguageService _languageService;

        private ILanguageService LanguageService
        {
            get
            {
                if (_languageService == null)
                {
                    _languageService = EngineContext.Current.Resolve<ILanguageService>();
                }
                return _languageService;
            }
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var localizedModelType = bindingContext.ModelType.GetGenericArguments()[0];
            var localized = Activator.CreateInstance(typeof(LocalizedModels<>).MakeGenericType(localizedModelType)) as IDictionary;
            foreach (var key in controllerContext.RequestContext.HttpContext.Request.Form.Keys)
            {
                var match = Regex.Match(key.ToString(), @"Localized\[([0-9\-]+)\]\.([A-Za-z0-9\-]+)$",
                    RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Language language = LanguageService.GetLanguageById(int.Parse(match.Groups[1].Value));
                    dynamic instance = localized[language.Id];
                    if (instance == null)
                    {
                        instance = Activator.CreateInstance(localizedModelType);
                        localized.Add(language.Id, instance);
                    }
                    
                    string property = match.Groups[2].Value;
                    string propertyValue = controllerContext.RequestContext.HttpContext.Request.Form[key.ToString()];
                    CommonHelper.SetProperty(instance, property, propertyValue);
                }
            }
            return localized;
        }
    }
}