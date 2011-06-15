﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Common
{
    public class StoreThemeSelectorModel : BaseNopModel
    {
        public StoreThemeSelectorModel()
        {
            AvailableStoreThemes = new List<StoreThemeModel>();
        }

        public IList<StoreThemeModel> AvailableStoreThemes { get; set; }

        public StoreThemeModel CurrentStoreTheme { get; set; }
    }
}