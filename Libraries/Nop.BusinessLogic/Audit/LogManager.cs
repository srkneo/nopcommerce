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
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Audit;

namespace NopSolutions.NopCommerce.BusinessLogic.Audit
{
    /// <summary>
    /// Log manager
    /// </summary>
    public partial class LogManager
    {
        #region Utilities
        private static LogCollection DBMapping(DBLogCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new LogCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Log DBMapping(DBLog dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Log();
            item.LogID = dbItem.LogID;
            item.LogTypeID = dbItem.LogTypeID;
            item.Severity = dbItem.Severity;
            item.Message = dbItem.Message;
            item.Exception = dbItem.Exception;
            item.IPAddress = dbItem.IPAddress;
            item.CustomerID = dbItem.CustomerID;
            item.PageURL = dbItem.PageURL;
            item.ReferrerURL = dbItem.ReferrerURL;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="LogID">Log item identifier</param>
        public static void DeleteLog(int LogID)
        {
            DBProviderManager<DBLogProvider>.Provider.DeleteLog(LogID);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public static void ClearLog()
        {
            DBProviderManager<DBLogProvider>.Provider.ClearLog();
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <returns>Log item collection</returns>
        public static LogCollection GetAllLogs()
        {
            var dbCollection = DBProviderManager<DBLogProvider>.Provider.GetAllLogs();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="LogID">Log item identifier</param>
        /// <returns>Log item</returns>
        public static Log GetLogByID(int LogID)
        {
            if (LogID == 0)
                return null;

            var dbItem = DBProviderManager<DBLogProvider>.Provider.GetLogByID(LogID);
            var log = DBMapping(dbItem);
            return log;
        }
        
        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="LogType">Log item type</param>
        /// <param name="Message">The short message</param>
        /// <param name="Exception">The exception</param>
        /// <returns>A log item</returns>
        public static Log InsertLog(LogTypeEnum LogType, string Message, string Exception)
        {
            return InsertLog(LogType, Message, new Exception(String.IsNullOrEmpty(Exception) ? string.Empty : Exception));
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="LogType">Log item type</param>
        /// <param name="Message">The short message</param>
        /// <param name="Exception">The exception</param>
        /// <returns>A log item</returns>
        public static Log InsertLog(LogTypeEnum LogType, string Message, Exception Exception)
        {
            int CustomerID = 0;
            if (NopContext.Current != null && NopContext.Current.User != null)
                CustomerID = NopContext.Current.User.CustomerID;
            string IPAddress = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request!=null)
                IPAddress = HttpContext.Current.Request.UserHostAddress;
            string PageURL = CommonHelper.GetThisPageURL(true);

            return InsertLog(LogType, 11, Message, Exception, IPAddress, CustomerID, PageURL);
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="LogType">Log item type</param>
        /// <param name="Severity">The severity</param>
        /// <param name="Message">The short message</param>
        /// <param name="exception">The full exception</param>
        /// <param name="IPAddress">The IP address</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="PageURL">The page URL</param>
        /// <returns>Log item</returns>
        public static Log InsertLog(LogTypeEnum LogType, int Severity, string Message,
            Exception exception, string IPAddress,
            int CustomerID, string PageURL)
        {
            //don't log thread abort exception
            if ((exception != null) && (exception is System.Threading.ThreadAbortException))
                return null;

            if (IPAddress == null)
                IPAddress = string.Empty;

            string ReferrerURL = string.Empty;
            if (HttpContext.Current != null &&
                HttpContext.Current.Request != null &&
                HttpContext.Current.Request.UrlReferrer != null)
                ReferrerURL = HttpContext.Current.Request.UrlReferrer.ToString();
            if (ReferrerURL == null)
                ReferrerURL = string.Empty;

            DateTime CreatedOn = DateTimeHelper.ConvertToUtcTime(DateTime.Now);

            var dbItem = DBProviderManager<DBLogProvider>.Provider.InsertLog((int)LogType, Severity, Message,
             exception == null ? string.Empty : exception.ToString(), IPAddress, 
             CustomerID, PageURL, ReferrerURL, CreatedOn);
            var log = DBMapping(dbItem);
            return log;
        }
        #endregion
    }
}
