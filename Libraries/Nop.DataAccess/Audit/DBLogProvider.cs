//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Acts as a base class for deriving custom log provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/LogProvider")]
    public abstract partial class DBLogProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        public abstract void DeleteLog(int logId);
        
        /// <summary>
        /// Clears a log
        /// </summary>
        public abstract void ClearLog();
        
        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <returns>Log item collection</returns>
        public abstract DBLogCollection GetAllLogs();

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public abstract DBLog GetLogById(int logId);

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logTypeId">Log item type identifier</param>
        /// <param name="severity">The severity</param>
        /// <param name="message">The short message</param>
        /// <param name="exception">The full exception</param>
        /// <param name="ipAddress">The IP address</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="pageUrl">The page URL</param>
        /// <param name="referrerUrl">The referrer URL</param>
        /// <param name="createdOn">The date and time of instance creationL</param>
        /// <returns>Log item</returns>
        public abstract DBLog InsertLog(int logTypeId, int severity, string message,
            string exception, string ipAddress, int customerId,
            string pageUrl, string referrerUrl, DateTime createdOn);
        #endregion
    }
}