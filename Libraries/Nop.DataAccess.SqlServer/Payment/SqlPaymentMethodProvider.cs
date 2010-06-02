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

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Payment method provider for SQL Server
    /// </summary>
    public partial class SqlPaymentMethodProvider : DBPaymentMethodProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBPaymentMethod GetPaymentMethodFromReader(IDataReader dataReader)
        {
            var item = new DBPaymentMethod();
            item.PaymentMethodId = NopSqlDataHelper.GetInt(dataReader, "PaymentMethodID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.VisibleName = NopSqlDataHelper.GetString(dataReader, "VisibleName");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.ConfigureTemplatePath = NopSqlDataHelper.GetString(dataReader, "ConfigureTemplatePath");
            item.UserTemplatePath = NopSqlDataHelper.GetString(dataReader, "UserTemplatePath");
            item.ClassName = NopSqlDataHelper.GetString(dataReader, "ClassName");
            item.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            item.IsActive = NopSqlDataHelper.GetBoolean(dataReader, "IsActive");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
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
        /// Deletes a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        public override void DeletePaymentMethod(int paymentMethodId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodDelete");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod GetPaymentMethodById(int paymentMethodId)
        {
            DBPaymentMethod item = null;
            if (paymentMethodId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPaymentMethodFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="systemKeyword">Payment method system keyword</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod GetPaymentMethodBySystemKeyword(string systemKeyword)
        {

            DBPaymentMethod item = null;
            if (String.IsNullOrEmpty(systemKeyword))
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodLoadBySystemKeyword");
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPaymentMethodFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <returns>Payment method collection</returns>
        public override DBPaymentMethodCollection GetAllPaymentMethods(bool showHidden,
            int? filterByCountryId)
        {
            var result = new DBPaymentMethodCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            if(filterByCountryId.HasValue)
            {
                db.AddInParameter(dbCommand, "FilterByCountryID", DbType.Int32, filterByCountryId.Value);
            }
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetPaymentMethodFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="visibleName">The visible name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="userTemplatePath">The user template path</param>
        /// <param name="className">The class name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="isActive">A value indicating whether the payment method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod InsertPaymentMethod(string name,
            string visibleName, string description, string configureTemplatePath,
            string userTemplatePath, string className, string systemKeyword,
            bool isActive, int displayOrder)
        {
            DBPaymentMethod item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodInsert");
            db.AddOutParameter(dbCommand, "PaymentMethodID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "VisibleName", DbType.String, visibleName);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, configureTemplatePath);
            db.AddInParameter(dbCommand, "UserTemplatePath", DbType.String, userTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, className);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int paymentMethodId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PaymentMethodID"));
                item = GetPaymentMethodById(paymentMethodId);
            }
            return item;
        }

        /// <summary>
        /// Updates the payment method
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifer</param>
        /// <param name="name">The name</param>
        /// <param name="visibleName">The visible name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="userTemplatePath">The user template path</param>
        /// <param name="className">The class name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="isActive">A value indicating whether the payment method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public override DBPaymentMethod UpdatePaymentMethod(int paymentMethodId,
            string name, string visibleName, string description, string configureTemplatePath,
            string userTemplatePath, string className, string systemKeyword,
            bool isActive, int displayOrder)
        {
            DBPaymentMethod item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethodUpdate");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "VisibleName", DbType.String, visibleName);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, configureTemplatePath);
            db.AddInParameter(dbCommand, "UserTemplatePath", DbType.String, userTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, className);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetPaymentMethodById(paymentMethodId);

            return item;
        }

        /// <summary>
        /// Inserts payment method country mapping
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public override void InsertPaymentMethodCountryMapping(int paymentMethodId, int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethod_RestrictedCountriesInsert");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Checking whether the payment method country mapping exists
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public override bool DoesPaymentMethodCountryMappingExist(int paymentMethodId, int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethod_RestrictedCountriesContains");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddOutParameter(dbCommand, "Result", DbType.Boolean, 0);
            db.ExecuteNonQuery(dbCommand);
            return Convert.ToBoolean(db.GetParameterValue(dbCommand, "@Result"));
        }

        /// <summary>
        /// Deletes payment method country mapping
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public override void DeletePaymentMethodCountryMapping(int paymentMethodId, int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PaymentMethod_RestrictedCountriesDelete");
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}
