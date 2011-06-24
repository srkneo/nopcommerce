﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Infrastructure;
using Nop.Services.Customers;

namespace Nop.Web.Framework.Controllers
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var user = workContext.CurrentCustomer;
            bool isAdmin = user != null && user.IsAdmin();
            return isAdmin;
        }
    }
}
