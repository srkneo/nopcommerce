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
namespace NopSolutions.NopCommerce.DataAccess.Promo.Discounts
{
    /// <summary>
    /// Discount provider for SQL Server
    /// </summary>
    public partial class SqlDiscountProvider : DBDiscountProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBDiscount GetDiscountFromReader(IDataReader dataReader)
        {
            var item = new DBDiscount();
            item.DiscountId = NopSqlDataHelper.GetInt(dataReader, "DiscountID");
            item.DiscountTypeId = NopSqlDataHelper.GetInt(dataReader, "DiscountTypeID");
            item.DiscountLimitationId = NopSqlDataHelper.GetInt(dataReader, "DiscountLimitationID");
            item.DiscountRequirementId = NopSqlDataHelper.GetInt(dataReader, "DiscountRequirementID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.UsePercentage = NopSqlDataHelper.GetBoolean(dataReader, "UsePercentage");
            item.DiscountPercentage = NopSqlDataHelper.GetDecimal(dataReader, "DiscountPercentage");
            item.DiscountAmount = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmount");
            item.StartDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "StartDate");
            item.EndDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "EndDate");
            item.RequiresCouponCode = NopSqlDataHelper.GetBoolean(dataReader, "RequiresCouponCode");
            item.CouponCode = NopSqlDataHelper.GetString(dataReader, "CouponCode");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            return item;
        }

        private DBDiscountUsageHistory GetDiscountUsageHistoryFromReader(IDataReader dataReader)
        {
            var item = new DBDiscountUsageHistory();
            item.DiscountUsageHistoryId = NopSqlDataHelper.GetInt(dataReader, "DiscountUsageHistoryID");
            item.DiscountId = NopSqlDataHelper.GetInt(dataReader, "DiscountID");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
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
        /// Gets all discount usage history entries
        /// </summary>
        /// <param name="discountId">Discount type identifier; null to load all</param>
        /// <param name="customerId">Customer identifier; null to load all</param>
        /// <param name="orderId">Order identifier; null to load all</param>
        /// <returns>Discount usage history entries</returns>
        public override DBDiscountUsageHistoryCollection GetAllDiscountUsageHistoryEntries(int? discountId,
            int? customerId, int? orderId)
        {
            var result = new DBDiscountUsageHistoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUsageHistoryLoadAll");
            if (discountId.HasValue)
                db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId.Value);
            else
                db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, null);
            if (customerId.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (orderId.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId.Value);
            else
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetDiscountUsageHistoryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        #endregion
    }
}
