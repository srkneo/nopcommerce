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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Payment;

namespace NopSolutions.NopCommerce.BusinessLogic.Payment
{
    /// <summary>
    /// Payment method manager
    /// </summary>
    public partial class PaymentMethodManager
    {
        #region Constants
        private const string PAYMENTMETHODS_BY_ID_KEY = "Nop.paymentmethod.id-{0}";
        private const string PAYMENTMETHODS_PATTERN_KEY = "Nop.paymentmethod.";
        #endregion

        #region Utilities
        private static PaymentMethodCollection DBMapping(DBPaymentMethodCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new PaymentMethodCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static PaymentMethod DBMapping(DBPaymentMethod dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new PaymentMethod();
            item.PaymentMethodID = dbItem.PaymentMethodID;
            item.Name = dbItem.Name;
            item.VisibleName = dbItem.VisibleName;
            item.Description = dbItem.Description;
            item.ConfigureTemplatePath = dbItem.ConfigureTemplatePath;
            item.UserTemplatePath = dbItem.UserTemplatePath;
            item.ClassName = dbItem.ClassName;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.IsActive = dbItem.IsActive;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        public static void DeletePaymentMethod(int PaymentMethodID)
        {
            DBProviderManager<DBPaymentMethodProvider>.Provider.DeletePaymentMethod(PaymentMethodID);

            if (PaymentMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PAYMENTMETHODS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Payment method</returns>
        public static PaymentMethod GetPaymentMethodByID(int PaymentMethodID)
        {
            if (PaymentMethodID == 0)
                return null;

            string key = string.Format(PAYMENTMETHODS_BY_ID_KEY, PaymentMethodID);
            object obj2 = NopCache.Get(key);
            if (PaymentMethodManager.CacheEnabled && (obj2 != null))
            {
                return (PaymentMethod)obj2;
            }

            var dbItem = DBProviderManager<DBPaymentMethodProvider>.Provider.GetPaymentMethodByID(PaymentMethodID);
            var paymentMethod = DBMapping(dbItem);

            if (PaymentMethodManager.CacheEnabled)
            {
                NopCache.Max(key, paymentMethod);
            }
            return paymentMethod;
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="SystemKeyword">Payment method system keyword</param>
        /// <returns>Payment method</returns>
        public static PaymentMethod GetPaymentMethodBySystemKeyword(string SystemKeyword)
        {
            var dbItem = DBProviderManager<DBPaymentMethodProvider>.Provider.GetPaymentMethodBySystemKeyword(SystemKeyword);
            var paymentMethod = DBMapping(dbItem);
            return paymentMethod;
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <returns>Payment method collection</returns>
        public static PaymentMethodCollection GetAllPaymentMethods()
        {
            return GetAllPaymentMethods(null);
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="FilterByCountryID">The country indentifier</param>
        /// <returns>Payment method collection</returns>
        public static PaymentMethodCollection GetAllPaymentMethods(int? FilterByCountryID)
        {
            bool showHidden = NopContext.Current.IsAdmin;

            return GetAllPaymentMethods(FilterByCountryID, showHidden);
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="FilterByCountryID">The country indentifier</param>
        /// <param name="ShowHidden">A value indicating whether the not active payment methods should be load</param>
        /// <returns>Payment method collection</returns>
        public static PaymentMethodCollection GetAllPaymentMethods(int? FilterByCountryID, bool ShowHidden)
        {
            var dbCollection = DBProviderManager<DBPaymentMethodProvider>.Provider.GetAllPaymentMethods(ShowHidden, FilterByCountryID);
            var paymentMethodCollection = DBMapping(dbCollection);

            return paymentMethodCollection;
        }

        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="VisibleName">The visible name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="UserTemplatePath">The user template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="IsActive">A value indicating whether the payment method is active</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public static PaymentMethod InsertPaymentMethod(string Name, string VisibleName, string Description,
           string ConfigureTemplatePath, string UserTemplatePath, string ClassName, string SystemKeyword, bool IsActive, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBPaymentMethodProvider>.Provider.InsertPaymentMethod(Name, VisibleName, 
                Description, ConfigureTemplatePath, UserTemplatePath, ClassName,
                SystemKeyword, IsActive, DisplayOrder);

            var paymentMethod = DBMapping(dbItem);
            if (PaymentMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PAYMENTMETHODS_PATTERN_KEY);
            }
            return paymentMethod;
        }

        /// <summary>
        /// Updates the payment method
        /// </summary>
        /// <param name="PaymentMethodID">The payment method identifer</param>
        /// <param name="Name">The name</param>
        /// <param name="VisibleName">The visible name</param>
        /// <param name="Description">The description</param>
        /// <param name="ConfigureTemplatePath">The configure template path</param>
        /// <param name="UserTemplatePath">The user template path</param>
        /// <param name="ClassName">The class name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="IsActive">A value indicating whether the payment method is active</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public static PaymentMethod UpdatePaymentMethod(int PaymentMethodID, string Name, string VisibleName, string Description,
           string ConfigureTemplatePath, string UserTemplatePath, string ClassName, string SystemKeyword, bool IsActive, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBPaymentMethodProvider>.Provider.UpdatePaymentMethod(PaymentMethodID, Name,
                VisibleName, Description, ConfigureTemplatePath, UserTemplatePath, 
                ClassName, SystemKeyword, IsActive, DisplayOrder);

            var paymentMethod = DBMapping(dbItem);

            if (PaymentMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PAYMENTMETHODS_PATTERN_KEY);
            }
            return paymentMethod;
        }

        /// <summary>
        /// Creates the payment method country mapping
        /// </summary>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        public static void CreatePaymentMethodCountryMapping(int PaymentMethodID, int CountryID)
        {
            DBProviderManager<DBPaymentMethodProvider>.Provider.InsertPaymentMethodCountryMapping(PaymentMethodID, CountryID);
        }

        /// <summary>
        /// Checking whether the payment method country mapping is exists
        /// </summary>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public static bool IsPaymentMethodCountryMappingExists(int PaymentMethodID, int CountryID)
        {
            return DBProviderManager<DBPaymentMethodProvider>.Provider.IsPaymentMethodCountryMappingExists(PaymentMethodID, CountryID);
        }

        /// <summary>
        /// Deletes the payment method country mapping
        /// </summary>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="CountryID">The country identifier</param>
        public static void DeletePaymentMethodCountryMapping(int PaymentMethodID, int CountryID)
        {
            DBProviderManager<DBPaymentMethodProvider>.Provider.DeletePaymentMethodCountryMapping(PaymentMethodID, CountryID);
        }
        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.PaymentMethodManager.CacheEnabled");
            }
        }
        #endregion
    }
}
