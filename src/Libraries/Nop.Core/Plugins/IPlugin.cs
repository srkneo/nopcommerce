﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Nop.Core.Plugins
{
    /// <summary>
    /// Interface denoting plug-in attributes that are displayed throughout 
    /// the editing interface.
    /// </summary>
    public interface IPlugin : IComparable<IPlugin>
    {
        string Name { get; set; }
        int SortOrder { get; }
        bool IsAuthorized(IPrincipal user);
    }
}
