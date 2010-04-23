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

namespace NopSolutions.NopCommerce.DataAccess.Tax
{
    /// <summary>
    /// Tax category provider for SQL Server
    /// </summary>
    public partial class SqlTaxCategoryProvider : DBTaxCategoryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTaxCategory GetTaxCategoryFromReader(IDataReader dataReader)
        {
            var item = new DBTaxCategory();
            item.TaxCategoryId = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
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
        /// Deletes a tax category
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        public override void DeleteTaxCategory(int taxCategoryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryDelete");
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all tax categories
        /// </summary>
        /// <returns>Tax category collection</returns>
        public override DBTaxCategoryCollection GetAllTaxCategories()
        {
            var result = new DBTaxCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetTaxCategoryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a tax category
        /// </summary>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <returns>Tax category</returns>
        public override DBTaxCategory GetTaxCategoryById(int taxCategoryId)
        {
            DBTaxCategory item = null;
            if (taxCategoryId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetTaxCategoryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a tax category
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public override DBTaxCategory InsertTaxCategory(string name,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBTaxCategory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryInsert");
            db.AddOutParameter(dbCommand, "TaxCategoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int taxCategoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TaxCategoryID"));
                item = GetTaxCategoryById(taxCategoryId);
            }
            return item;
        }

        /// <summary>
        /// Updates the tax category
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public override DBTaxCategory UpdateTaxCategory(int taxCategoryId, string name,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBTaxCategory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TaxCategoryUpdate");
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetTaxCategoryById(taxCategoryId);

            return item;
        }
        #endregion
    }
}
