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

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Shopping cart provider for SQL Server
    /// </summary>
    public partial class SqlShoppingCartProvider : DBShoppingCartProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShoppingCartItem GetShoppingCartItemFromReader(IDataReader dataReader)
        {
            var item = new DBShoppingCartItem();
            item.ShoppingCartItemId = NopSqlDataHelper.GetInt(dataReader, "ShoppingCartItemID");
            item.ShoppingCartTypeId = NopSqlDataHelper.GetInt(dataReader, "ShoppingCartTypeID");
            item.CustomerSessionGuid = NopSqlDataHelper.GetGuid(dataReader, "CustomerSessionGUID");
            item.ProductVariantId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.AttributesXml = NopSqlDataHelper.GetString(dataReader, "AttributesXML");
            item.CustomerEnteredPrice = NopSqlDataHelper.GetDecimal(dataReader, "CustomerEnteredPrice");
            item.Quantity = NopSqlDataHelper.GetInt(dataReader, "Quantity");
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
        /// Deletes expired shopping cart items
        /// </summary>
        /// <param name="olderThan">Older than date and time</param>
        public override void DeleteExpiredShoppingCartItems(DateTime olderThan)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemDeleteExpired");
            db.AddInParameter(dbCommand, "OlderThan", DbType.DateTime, olderThan);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a shopping cart item
        /// </summary>
        /// <param name="shoppingCartItemId">The shopping cart item identifier</param>
        public override void DeleteShoppingCartItem(int shoppingCartItemId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemDelete");
            db.AddInParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, shoppingCartItemId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a shopping cart by customer session GUID
        /// </summary>
        /// <param name="shoppingCartTypeId">Shopping cart type identifier</param>
        /// <param name="customerSessionGuid">The customer session identifier</param>
        /// <returns>Cart</returns>
        public override DBShoppingCart GetShoppingCartByCustomerSessionGuid(int shoppingCartTypeId,
            Guid customerSessionGuid)
        {
            var result = new DBShoppingCart();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemLoadByCustomerSessionGUID");
            db.AddInParameter(dbCommand, "ShoppingCartTypeID", DbType.Int32, shoppingCartTypeId);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, customerSessionGuid);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetShoppingCartItemFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a shopping cart item
        /// </summary>
        /// <param name="shoppingCartItemId">The shopping cart item identifier</param>
        /// <returns>Shopping cart item</returns>
        public override DBShoppingCartItem GetShoppingCartItemById(int shoppingCartItemId)
        {
            DBShoppingCartItem item = null;
            if (shoppingCartItemId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, shoppingCartItemId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetShoppingCartItemFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a shopping cart item
        /// </summary>
        /// <param name="shoppingCartTypeId">The shopping cart type identifier</param>
        /// <param name="customerSessionGuid">The customer session identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="attributesXml">The product variant attributes</param>
        /// <param name="customerEnteredPrice">The price enter by a customer</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        public override DBShoppingCartItem InsertShoppingCartItem(int shoppingCartTypeId,
            Guid customerSessionGuid, int productVariantId, string attributesXml,
            decimal customerEnteredPrice, int quantity,
            DateTime createdOn, DateTime updatedOn)
        {
            DBShoppingCartItem item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemInsert");
            db.AddOutParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ShoppingCartTypeID", DbType.Int32, shoppingCartTypeId);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, customerSessionGuid);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, attributesXml);
            db.AddInParameter(dbCommand, "CustomerEnteredPrice", DbType.Decimal, customerEnteredPrice);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, quantity);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int shoppingCartItemId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShoppingCartItemID"));
                item = GetShoppingCartItemById(shoppingCartItemId);
            }
            return item;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="shoppingCartItemId">The shopping cart item identifier</param>
        /// <param name="shoppingCartTypeId">The shopping cart type identifier</param>
        /// <param name="customerSessionGuid">The customer session identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="attributesXml">The product variant attributes</param>
        /// <param name="customerEnteredPrice">The price enter by a customer</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        public override DBShoppingCartItem UpdateShoppingCartItem(int shoppingCartItemId,
            int shoppingCartTypeId, Guid customerSessionGuid,
            int productVariantId, string attributesXml,
            decimal customerEnteredPrice, int quantity, DateTime createdOn, DateTime updatedOn)
        {
            DBShoppingCartItem item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShoppingCartItemUpdate");
            db.AddInParameter(dbCommand, "ShoppingCartItemID", DbType.Int32, shoppingCartItemId);
            db.AddInParameter(dbCommand, "ShoppingCartTypeID", DbType.Int32, shoppingCartTypeId);
            db.AddInParameter(dbCommand, "CustomerSessionGUID", DbType.Guid, customerSessionGuid);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, attributesXml);
            db.AddInParameter(dbCommand, "CustomerEnteredPrice", DbType.Decimal, customerEnteredPrice);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, quantity);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetShoppingCartItemById(shoppingCartItemId);
            
            return item;
        }
        #endregion
    }
}

