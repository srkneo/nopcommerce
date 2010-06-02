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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Affiliates
{
    /// <summary>
    /// Affiliate provider for SQL Server
    /// </summary>
    public partial class SqlAffiliateProvider : DBAffiliateProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBAffiliate GetAffiliateFromReader(IDataReader dataReader)
        {
            var item = new DBAffiliate();
            item.AffiliateId = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            item.FirstName = NopSqlDataHelper.GetString(dataReader, "FirstName");
            item.LastName = NopSqlDataHelper.GetString(dataReader, "LastName");
            item.MiddleName = NopSqlDataHelper.GetString(dataReader, "MiddleName");
            item.PhoneNumber = NopSqlDataHelper.GetString(dataReader, "PhoneNumber");
            item.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            item.FaxNumber = NopSqlDataHelper.GetString(dataReader, "FaxNumber");
            item.Company = NopSqlDataHelper.GetString(dataReader, "Company");
            item.Address1 = NopSqlDataHelper.GetString(dataReader, "Address1");
            item.Address2 = NopSqlDataHelper.GetString(dataReader, "Address2");
            item.City = NopSqlDataHelper.GetString(dataReader, "City");
            item.StateProvince = NopSqlDataHelper.GetString(dataReader, "StateProvince");
            item.ZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ZipPostalCode");
            item.CountryId = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.Active = NopSqlDataHelper.GetBoolean(dataReader, "Active");
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
        /// Gets an affiliate by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Affiliate</returns>
        public override DBAffiliate GetAffiliateById(int affiliateId)
        {
            DBAffiliate item = null;
            if (affiliateId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, affiliateId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetAffiliateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all affiliates
        /// </summary>
        /// <returns>Affiliate collection</returns>
        public override DBAffiliateCollection GetAllAffiliates()
        {
            var result = new DBAffiliateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetAffiliateFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts an affiliate
        /// </summary>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="middleName">The middle name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="company">The company</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvince">The state/province</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="active">A value indicating whether the entity is active</param>
        /// <returns>An affiliate</returns>
        public override DBAffiliate InsertAffiliate(string firstName,
            string lastName, string middleName, string phoneNumber,
            string email, string faxNumber, string company, string address1,
            string address2, string city, string stateProvince, string zipPostalCode,
            int countryId, bool deleted, bool active)
        {
            DBAffiliate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateInsert");
            db.AddOutParameter(dbCommand, "AffiliateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "FirstName", DbType.String, firstName);
            db.AddInParameter(dbCommand, "LastName", DbType.String, lastName);
            db.AddInParameter(dbCommand, "MiddleName", DbType.String, middleName);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, phoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, faxNumber);
            db.AddInParameter(dbCommand, "Company", DbType.String, company);
            db.AddInParameter(dbCommand, "Address1", DbType.String, address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, address2);
            db.AddInParameter(dbCommand, "City", DbType.String, city);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, stateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, zipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, active);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int affiliateId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@AffiliateID"));
                item = GetAffiliateById(affiliateId);
            }
            return item;
        }

        /// <summary>
        /// Updates the affiliate
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="middleName">The middle name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="company">The company</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvince">The state/province</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="active">A value indicating whether the entity is active</param>
        /// <returns>An affiliate</returns>
        public override DBAffiliate UpdateAffiliate(int affiliateId, string firstName,
            string lastName, string middleName, string phoneNumber,
            string email, string faxNumber, string company, string address1,
            string address2, string city, string stateProvince, string zipPostalCode,
            int countryId, bool deleted, bool active)
        {
            DBAffiliate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_AffiliateUpdate");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, affiliateId);
            db.AddInParameter(dbCommand, "FirstName", DbType.String, firstName);
            db.AddInParameter(dbCommand, "LastName", DbType.String, lastName);
            db.AddInParameter(dbCommand, "MiddleName", DbType.String, middleName);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, phoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, faxNumber);
            db.AddInParameter(dbCommand, "Company", DbType.String, company);
            db.AddInParameter(dbCommand, "Address1", DbType.String, address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, address2);
            db.AddInParameter(dbCommand, "City", DbType.String, city);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, stateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, zipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, active);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetAffiliateById(affiliateId);

            return item;
        }

        #endregion
    }
}
