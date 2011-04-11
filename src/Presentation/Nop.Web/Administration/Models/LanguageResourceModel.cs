﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation.Attributes;
using Nop.Admin.Validators;
using Nop.Core.Domain.Localization;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models
{
    [Validator(typeof(LanguageResourceValidator))]
    public class LanguageResourceModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Configuration.Location.Languages.Resources.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Location.Languages.Resources.Fields.Value")]
        public string Value { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Location.Languages.Resources.Fields.LanguageName")]
        public string LanguageName { get; set; }

        public int LanguageId { get; set; }
    }
}