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
    /// Shipping status provider for SQL Server
    /// </summary>
    public partial class SqlShippingStatusProvider : DBShippingStatusProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingStatus GetShippingStatusFromReader(IDataReader dataReader)
        {
            var item = new DBShippingStatus();
            item.ShippingStatusId = NopSqlDataHelper.GetInt(dataReader, "ShippingStatusID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
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
        /// Gets a shipping status by identifier
        /// </summary>
        /// <param name="shippingStatusId">Shipping status identifier</param>
        /// <returns>Shipping status</returns>
        public override DBShippingStatus GetShippingStatusById(int shippingStatusId)
        {
            DBShippingStatus item = null;
            if (shippingStatusId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingStatusLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetShippingStatusFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all shipping statuses
        /// </summary>
        /// <returns>Shipping status collection</returns>
        public override DBShippingStatusCollection GetAllShippingStatuses()
        {
            var result = new DBShippingStatusCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingStatusLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetShippingStatusFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        #endregion
    }
}
