using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Nop.Web.MVC.Areas.Admin.Controllers
{
    //[Dependency(typeof(ISettings))]
    public class ServiceWithSettings : ISettings
    {
        public ServiceWithSettings(IWorkContext workContext)
        {
        
        }
        public string ServiceSettingValue { get; set; }
    }
}