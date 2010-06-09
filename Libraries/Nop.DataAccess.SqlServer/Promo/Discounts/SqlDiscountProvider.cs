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
        /// Gets a discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <returns>Discount</returns>
        public override DBDiscount GetDiscountById(int discountId)
        {
            DBDiscount item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetDiscountFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="discountTypeId">Discount type identifier; null to load all discount</param>
        /// <returns>Discount collection</returns>
        public override DBDiscountCollection GetAllDiscounts(bool showHidden, int? discountTypeId)
        {
            var result = new DBDiscountCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            if (discountTypeId.HasValue)
                db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, discountTypeId.Value);
            else
                db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetDiscountFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="discountTypeId">The discount type identifier</param>
        /// <param name="discountRequirementId">The discount requirement identifier</param>
        /// <param name="discountLimitationId">The discount limitation identifier</param>
        /// <param name="name">The name</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="discountPercentage">The discount percentage</param>
        /// <param name="discountAmount">The discount amount</param>
        /// <param name="startDate">The discount start date and time</param>
        /// <param name="endDate">The discount end date and time</param>
        /// <param name="requiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="couponCode">The coupon code</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public override DBDiscount InsertDiscount(int discountTypeId,
            int discountRequirementId, int discountLimitationId,
            string name, bool usePercentage, decimal discountPercentage,
            decimal discountAmount, DateTime startDate, DateTime endDate,
            bool requiresCouponCode, string couponCode, bool deleted)
        {
            DBDiscount item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountInsert");
            db.AddOutParameter(dbCommand, "DiscountID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, discountTypeId);
            db.AddInParameter(dbCommand, "DiscountRequirementID", DbType.Int32, discountRequirementId);
            db.AddInParameter(dbCommand, "DiscountLimitationID", DbType.Int32, discountLimitationId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, usePercentage);
            db.AddInParameter(dbCommand, "DiscountPercentage", DbType.Decimal, discountPercentage);
            db.AddInParameter(dbCommand, "DiscountAmount", DbType.Decimal, discountAmount);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
            db.AddInParameter(dbCommand, "RequiresCouponCode", DbType.Boolean, requiresCouponCode);
            db.AddInParameter(dbCommand, "CouponCode", DbType.String, couponCode);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int discountId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@DiscountID"));
                item = GetDiscountById(discountId);
            }
            return item;
        }

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="discountTypeId">The discount type identifier</param>
        /// <param name="discountRequirementId">The discount requirement identifier</param>
        /// <param name="discountLimitationId">The discount limitation identifier</param>
        /// <param name="name">The name</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="discountPercentage">The discount percentage</param>
        /// <param name="discountAmount">The discount amount</param>
        /// <param name="startDate">The discount start date and time</param>
        /// <param name="endDate">The discount end date and time</param>
        /// <param name="requiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="couponCode">The coupon code</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public override DBDiscount UpdateDiscount(int discountId, int discountTypeId,
            int discountRequirementId, int discountLimitationId,
            string name, bool usePercentage, decimal discountPercentage,
            decimal discountAmount, DateTime startDate, DateTime endDate,
            bool requiresCouponCode, string couponCode, bool deleted)
        {
            DBDiscount item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUpdate");
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.AddInParameter(dbCommand, "DiscountTypeID", DbType.Int32, discountTypeId);
            db.AddInParameter(dbCommand, "DiscountRequirementID", DbType.Int32, discountRequirementId);
            db.AddInParameter(dbCommand, "DiscountLimitationID", DbType.Int32, discountLimitationId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, usePercentage);
            db.AddInParameter(dbCommand, "DiscountPercentage", DbType.Decimal, discountPercentage);
            db.AddInParameter(dbCommand, "DiscountAmount", DbType.Decimal, discountAmount);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
            db.AddInParameter(dbCommand, "EndDate", DbType.DateTime, endDate);
            db.AddInParameter(dbCommand, "RequiresCouponCode", DbType.Boolean, requiresCouponCode);
            db.AddInParameter(dbCommand, "CouponCode", DbType.String, couponCode);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetDiscountById(discountId);

            return item;
        }

        /// <summary>
        /// Adds a discount to a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void AddDiscountToProductVariant(int productVariantId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Discount_MappingInsert");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount from a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void RemoveDiscountFromProductVariant(int productVariantId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Discount_MappingDelete");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a discount collection of a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public override DBDiscountCollection GetDiscountsByProductVariantId(int productVariantId, bool showHidden)
        {
            var result = new DBDiscountCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadByProductVariantID");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetDiscountFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a discount to a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void AddDiscountToCategory(int categoryId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Category_Discount_MappingInsert");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount from a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void RemoveDiscountFromCategory(int categoryId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Category_Discount_MappingDelete");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a discount collection of a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public override DBDiscountCollection GetDiscountsByCategoryId(int categoryId, bool showHidden)
        {
            var result = new DBDiscountCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountLoadByCategoryID");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetDiscountFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a discount requirement
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void AddDiscountRestriction(int productVariantId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountRestrictionInsert");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes discount requirement
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public override void RemoveDiscountRestriction(int productVariantId, int discountId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountRestrictionDelete");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a discount usage history entry
        /// </summary>
        /// <param name="discountUsageHistoryId">Discount usage history entry identifier</param>
        public override void DeleteDiscountUsageHistory(int discountUsageHistoryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUsageHistoryDelete");
            db.AddInParameter(dbCommand, "DiscountUsageHistoryID", DbType.Int32, discountUsageHistoryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a discount usage history entry
        /// </summary>
        /// <param name="discountUsageHistoryId">Discount usage history entry identifier</param>
        /// <returns>Discount usage history entry</returns>
        public override DBDiscountUsageHistory GetDiscountUsageHistoryById(int discountUsageHistoryId)
        {
            DBDiscountUsageHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUsageHistoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "DiscountUsageHistoryID", DbType.Int32, discountUsageHistoryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetDiscountUsageHistoryFromReader(dataReader);
                }
            }
            return item;
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

        /// <summary>
        /// Inserts a discount usage history entry
        /// </summary>
        /// <param name="discountId">Discount type identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Discount usage history entry</returns>
        public override DBDiscountUsageHistory InsertDiscountUsageHistory(int discountId,
            int customerId, int orderId, DateTime createdOn)
        {
            DBDiscountUsageHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUsageHistoryInsert");
            db.AddOutParameter(dbCommand, "DiscountUsageHistoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int discountUsageHistoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@DiscountUsageHistoryID"));
                item = GetDiscountUsageHistoryById(discountUsageHistoryId);
            }
            return item;
        }

        /// <summary>
        /// Updates the discount usage history entry
        /// </summary>
        /// <param name="discountUsageHistoryId">discount usage history entry identifier</param>
        /// <param name="discountId">Discount type identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Discount usage history entry</returns>
        public override DBDiscountUsageHistory UpdateDiscountUsageHistory(int discountUsageHistoryId,
            int discountId, int customerId, int orderId, DateTime createdOn)
        {
            DBDiscountUsageHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DiscountUsageHistoryUpdate");
            db.AddInParameter(dbCommand, "DiscountUsageHistoryID", DbType.Int32, discountUsageHistoryId);
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetDiscountUsageHistoryById(discountUsageHistoryId);

            return item;
        }

        #endregion
    }
}
