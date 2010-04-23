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

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Shipping rate computation method provider for SQL Server
    /// </summary>
    public partial class SqlShippingRateComputationMethodProvider : DBShippingRateComputationMethodProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingRateComputationMethod GetShippingRateComputationMethodFromReader(IDataReader dataReader)
        {
            var item = new DBShippingRateComputationMethod();
            item.ShippingRateComputationMethodId = NopSqlDataHelper.GetInt(dataReader, "ShippingRateComputationMethodID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.ConfigureTemplatePath = NopSqlDataHelper.GetString(dataReader, "ConfigureTemplatePath");
            item.ClassName = NopSqlDataHelper.GetString(dataReader, "ClassName");
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
        /// Deletes a shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">Shipping rate computation method identifier</param>
        public override void DeleteShippingRateComputationMethod(int shippingRateComputationMethodId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodDelete");
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, shippingRateComputationMethodId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">Shipping rate computation method identifier</param>
        /// <returns>Shipping rate computation method</returns>
        public override DBShippingRateComputationMethod GetShippingRateComputationMethodById(int shippingRateComputationMethodId)
        {
            DBShippingRateComputationMethod item = null;
            if (shippingRateComputationMethodId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, shippingRateComputationMethodId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetShippingRateComputationMethodFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all shipping rate computation methods
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Shipping rate computation method collection</returns>
        public override DBShippingRateComputationMethodCollection GetAllShippingRateComputationMethods(bool showHidden)
        {
            var result = new DBShippingRateComputationMethodCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetShippingRateComputationMethodFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a shipping rate computation method
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="isActive">The value indicating whether the method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public override DBShippingRateComputationMethod InsertShippingRateComputationMethod(string name,
            string description, string configureTemplatePath, string className,
            bool isActive, int displayOrder)
        {
            DBShippingRateComputationMethod item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodInsert");
            db.AddOutParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, configureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, className);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int shippingRateComputationMethodId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingRateComputationMethodID"));
                item = GetShippingRateComputationMethodById(shippingRateComputationMethodId);
            }
            return item;
        }

        /// <summary>
        /// Updates the shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">The shipping rate computation method identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="isActive">The value indicating whether the method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public override DBShippingRateComputationMethod UpdateShippingRateComputationMethod(int shippingRateComputationMethodId,
            string name, string description, string configureTemplatePath, string className,
            bool isActive, int displayOrder)
        {
            DBShippingRateComputationMethod item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingRateComputationMethodUpdate");
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, shippingRateComputationMethodId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "ConfigureTemplatePath", DbType.String, configureTemplatePath);
            db.AddInParameter(dbCommand, "ClassName", DbType.String, className);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetShippingRateComputationMethodById(shippingRateComputationMethodId);

            return item;
        }
        #endregion
    }
}
