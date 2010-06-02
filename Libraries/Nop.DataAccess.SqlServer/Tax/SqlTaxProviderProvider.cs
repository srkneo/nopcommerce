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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Tax
{
    /// <summary>
    /// Tax provider provider for SQL Server
    /// </summary>
    public partial class SqlTaxProviderProvider : DBTaxProviderProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTaxProvider GetTaxProviderFromReader(IDataReader dataReader)
        {
            var item = new DBTaxProvider();
            item.TaxProviderId = NopSqlDataHelper.GetInt(dataReader, "TaxProviderID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.ConfigureTemplatePath = NopSqlDataHelper.GetString(dataReader, "ConfigureTemplatePath");
            item.ClassName = NopSqlDataHelper.GetString(dataReader, "ClassName");
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
        /// Deletes a tax provider
        /// </summary>
        /// <param name="taxProviderId">Tax provider identifier</param>
        public override void DeleteTaxProvider(int taxProviderId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderDelete");
            db.AddInParameter(dbCommand, "TaxProviderID", DbType.Int32, taxProviderId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a tax provider
        /// </summary>
        /// <param name="taxProviderId">Tax provider identifier</param>
        /// <returns>Tax provider</returns>
        public override DBTaxProvider GetTaxProviderById(int taxProviderId)
        {
            DBTaxProvider item = null;
            if (taxProviderId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TaxProviderID", DbType.Int32, taxProviderId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetTaxProviderFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all tax providers
        /// </summary>
        /// <returns>Shipping rate computation method collection</returns>
        public override DBTaxProviderCollection GetAllTaxProviders()
        {
            var result = new DBTaxProviderCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetTaxProviderFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a tax provider
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public override DBTaxProvider InsertTaxProvider(string name, string description,
           string configureTemplatePath, string className, int displayOrder)
        {
            DBTaxProvider item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderInsert");
            db.AddOutParameter(dbCommand, "TaxProviderID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, configureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, className);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int taxProviderId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TaxProviderID"));
                item = GetTaxProviderById(taxProviderId);
            }
            return item;
        }

        /// <summary>
        /// Updates the tax provider
        /// </summary>
        /// <param name="taxProviderId">The tax provider identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public override DBTaxProvider UpdateTaxProvider(int taxProviderId,
            string name, string description, string configureTemplatePath,
            string className, int displayOrder)
        {
            DBTaxProvider item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxProviderUpdate");
            db.AddInParameter(dbCommand, "TaxProviderID", DbType.Int32, taxProviderId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, configureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, className);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetTaxProviderById(taxProviderId);

            return item;
        }
        #endregion
    }
}
