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
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.CustomerManagement
{
    /// <summary>
    /// Customer provider for SQL Server
    /// </summary>
    public partial class SqlCustomerProvider : DBCustomerProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        private DBCustomer GetCustomerFromReader(IDataReader dataReader)
        {
            var item = new DBCustomer();
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.CustomerGuid = NopSqlDataHelper.GetGuid(dataReader, "CustomerGUID");
            item.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            item.Username = NopSqlDataHelper.GetString(dataReader, "Username");
            item.PasswordHash = NopSqlDataHelper.GetString(dataReader, "PasswordHash");
            item.SaltKey = NopSqlDataHelper.GetString(dataReader, "SaltKey");
            item.AffiliateId = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            item.BillingAddressId = NopSqlDataHelper.GetInt(dataReader, "BillingAddressID");
            item.ShippingAddressId = NopSqlDataHelper.GetInt(dataReader, "ShippingAddressID");
            item.LastPaymentMethodId = NopSqlDataHelper.GetInt(dataReader, "LastPaymentMethodID");
            item.LastAppliedCouponCode = NopSqlDataHelper.GetString(dataReader, "LastAppliedCouponCode");
            item.GiftCardCouponCodes = NopSqlDataHelper.GetString(dataReader, "GiftCardCouponCodes");
            item.CheckoutAttributes = NopSqlDataHelper.GetString(dataReader, "CheckoutAttributes");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.CurrencyId = NopSqlDataHelper.GetInt(dataReader, "CurrencyID");
            item.TaxDisplayTypeId = NopSqlDataHelper.GetInt(dataReader, "TaxDisplayTypeID");
            item.IsAdmin = NopSqlDataHelper.GetBoolean(dataReader, "IsAdmin");
            item.IsTaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "IsTaxExempt");
            item.IsGuest = NopSqlDataHelper.GetBoolean(dataReader, "IsGuest");
            item.IsForumModerator = NopSqlDataHelper.GetBoolean(dataReader, "IsForumModerator");
            item.TotalForumPosts = NopSqlDataHelper.GetInt(dataReader, "TotalForumPosts");
            item.Signature = NopSqlDataHelper.GetString(dataReader, "Signature");
            item.AdminComment = NopSqlDataHelper.GetString(dataReader, "AdminComment");
            item.Active = NopSqlDataHelper.GetBoolean(dataReader, "Active");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.RegistrationDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "RegistrationDate");
            item.TimeZoneId = NopSqlDataHelper.GetString(dataReader, "TimeZoneID");
            item.AvatarId = NopSqlDataHelper.GetInt(dataReader, "AvatarID");
            return item;
        }

        private DBCustomerRole GetCustomerRoleFromReader(IDataReader dataReader)
        {
            var item = new DBCustomerRole();
            item.CustomerRoleId = NopSqlDataHelper.GetInt(dataReader, "CustomerRoleID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.FreeShipping = NopSqlDataHelper.GetBoolean(dataReader, "FreeShipping");
            item.TaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "TaxExempt");
            item.Active = NopSqlDataHelper.GetBoolean(dataReader, "Active");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="registrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="registrationTo">Customer registration to; null to load all customers</param>
        /// <param name="email">Customer Email</param>
        /// <param name="username">Customer username</param>
        /// <param name="dontLoadGuestCustomers">A value indicating whether to don't load guest customers</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Customer collection</returns>
        public override DBCustomerCollection GetAllCustomers(DateTime? registrationFrom,
            DateTime? registrationTo, string email, string username,
            bool dontLoadGuestCustomers, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBCustomerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerLoadAll");
            if (registrationFrom.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, registrationFrom.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (registrationTo.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, registrationTo.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "Username", DbType.String, username);
            db.AddInParameter(dbCommand, "DontLoadGuestCustomers", DbType.Boolean, dontLoadGuestCustomers);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCustomerFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Get best customers
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all records</param>
        /// <param name="orderBy">1 - order by order total, 2 - order by number of orders</param>
        /// <returns>Report</returns>
        public override IDataReader GetBestCustomersReport(DateTime? startTime,
            DateTime? endTime, int? orderStatusId, int? paymentStatusId,
            int? shippingStatusId, int orderBy)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerBestReport");
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            if (orderStatusId.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (paymentStatusId.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (shippingStatusId.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);
            db.AddInParameter(dbCommand, "OrderBy", DbType.Int32, orderBy);
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Get customer report by language
        /// </summary>
        /// <returns>Report</returns>
        public override IDataReader GetCustomerReportByLanguage()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerReportByLanguage");
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Get customer report by attribute key
        /// </summary>
        /// <param name="customerAttributeKey">Customer attribute key</param>
        /// <returns>Report</returns>
        public override IDataReader GetCustomerReportByAttributeKey(string customerAttributeKey)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerReportByAttributeKey");
            db.AddInParameter(dbCommand, "CustomerAttributeKey", DbType.String, customerAttributeKey);
            return db.ExecuteReader(dbCommand);
        }
        
        /// <summary>
        /// Adds a customer to role
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="customerRoleId">Customer role identifier</param>
        public override void AddCustomerToRole(int customerId, int customerRoleId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Customer_CustomerRole_MappingInsert");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a customer from role
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="customerRoleId">Customer role identifier</param>
        public override void RemoveCustomerFromRole(int customerId, int customerRoleId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Customer_CustomerRole_MappingDelete");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Adds a discount to a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void AddDiscountToCustomerRole(int customerRoleId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRole_Discount_MappingInsert");
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount from a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void RemoveDiscountFromCustomerRole(int customerRoleId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRole_Discount_MappingDelete");
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a customer roles assigned to discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public override DBCustomerRoleCollection GetCustomerRolesByDiscountId(int discountId, bool showHidden)
        {
            var result = new DBCustomerRoleCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRoleLoadByDiscountID");
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCustomerRoleFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes all expired customer sessions
        /// </summary>
        /// <param name="olderThan">Older than date and time</param>
        public override void DeleteExpiredCustomerSessions(DateTime olderThan)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerSessionDeleteExpired");
            db.AddInParameter(dbCommand, "OlderThan", DbType.DateTime, olderThan);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a report of customers registered from "dateTime" until today
        /// </summary>
        /// <param name="dateFrom">Customer registration date</param>
        /// <returns>Int</returns>
        public override int GetRegisteredCustomersReport(DateTime dateFrom)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerRegisteredReport");
            db.AddInParameter(dbCommand, "Date", DbType.DateTime, dateFrom);
            return (int)db.ExecuteScalar(dbCommand);
        }
        #endregion
    }
}
