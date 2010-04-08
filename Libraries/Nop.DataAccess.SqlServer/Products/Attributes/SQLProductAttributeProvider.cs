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

namespace NopSolutions.NopCommerce.DataAccess.Products.Attributes
{
    /// <summary>
    /// Product attribute provider for SQL Server
    /// </summary>
    public partial class SQLProductAttributeProvider : DBProductAttributeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBProductAttribute GetProductAttributeFromReader(IDataReader dataReader)
        {
            DBProductAttribute productAttribute = new DBProductAttribute();
            productAttribute.ProductAttributeID = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeID");
            productAttribute.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            productAttribute.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            return productAttribute;
        }

        private DBProductAttributeLocalized GetProductAttributeLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBProductAttributeLocalized();
            item.ProductAttributeLocalizedID = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeLocalizedID");
            item.ProductAttributeID = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeID");
            item.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            return item;
        }

        private DBProductVariantAttribute GetProductVariantAttributeFromReader(IDataReader dataReader)
        {
            DBProductVariantAttribute productVariantAttribute = new DBProductVariantAttribute();
            productVariantAttribute.ProductVariantAttributeID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeID");
            productVariantAttribute.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            productVariantAttribute.ProductAttributeID = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeID");
            productVariantAttribute.TextPrompt = NopSqlDataHelper.GetString(dataReader, "TextPrompt");
            productVariantAttribute.IsRequired = NopSqlDataHelper.GetBoolean(dataReader, "IsRequired");
            productVariantAttribute.AttributeControlTypeID = NopSqlDataHelper.GetInt(dataReader, "AttributeControlTypeID");
            productVariantAttribute.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return productVariantAttribute;
        }

        private DBProductVariantAttributeValue GetProductVariantAttributeValueFromReader(IDataReader dataReader)
        {
            DBProductVariantAttributeValue productVariantAttributeValue = new DBProductVariantAttributeValue();
            productVariantAttributeValue.ProductVariantAttributeValueID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeValueID");
            productVariantAttributeValue.ProductVariantAttributeID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeID");
            productVariantAttributeValue.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            productVariantAttributeValue.PriceAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "PriceAdjustment");
            productVariantAttributeValue.WeightAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "WeightAdjustment");
            productVariantAttributeValue.IsPreSelected = NopSqlDataHelper.GetBoolean(dataReader, "IsPreSelected");
            productVariantAttributeValue.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return productVariantAttributeValue;
        }

        private DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBProductVariantAttributeValueLocalized();
            item.ProductVariantAttributeValueLocalizedID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeValueLocalizedID");
            item.ProductVariantAttributeValueID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeValueID");
            item.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return item;
        }

        private DBProductVariantAttributeCombination GetProductVariantAttributeCombinationFromReader(IDataReader dataReader)
        {
            DBProductVariantAttributeCombination item = new DBProductVariantAttributeCombination();
            item.ProductVariantAttributeCombinationID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeCombinationID");
            item.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.AttributesXML = NopSqlDataHelper.GetString(dataReader, "AttributesXML");
            item.StockQuantity = NopSqlDataHelper.GetInt(dataReader, "StockQuantity");
            item.AllowOutOfStockOrders = NopSqlDataHelper.GetBoolean(dataReader, "AllowOutOfStockOrders");
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
        /// Deletes a product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        public override void DeleteProductAttribute(int ProductAttributeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeDelete");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product attribute collection</returns>
        public override DBProductAttributeCollection GetAllProductAttributes(int LanguageID)
        {
            var result = new DBProductAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductAttributeFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product attribute </returns>
        public override DBProductAttribute GetProductAttributeByID(int ProductAttributeID, int LanguageID)
        {
            DBProductAttribute productAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productAttribute = GetProductAttributeFromReader(dataReader);
                }
            }
            return productAttribute;
        }

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public override DBProductAttribute InsertProductAttribute(string Name, string Description)
        {
            DBProductAttribute productAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeInsert");
            db.AddOutParameter(dbCommand, "ProductAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductAttributeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductAttributeID"));
                productAttribute = GetProductAttributeByID(ProductAttributeID, 0);
            }
            return productAttribute;
        }

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public override DBProductAttribute UpdateProductAttribute(int ProductAttributeID, string Name,
            string Description)
        {
            DBProductAttribute productAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeUpdate");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productAttribute = GetProductAttributeByID(ProductAttributeID, 0);
            return productAttribute;
        }

        /// <summary>
        /// Gets localized product attribute by id
        /// </summary>
        /// <param name="ProductAttributeLocalizedID">Localized product attribute identifier</param>
        /// <returns>Product attribute content</returns>
        public override DBProductAttributeLocalized GetProductAttributeLocalizedByID(int ProductAttributeLocalizedID)
        {
            DBProductAttributeLocalized item = null;
            if (ProductAttributeLocalizedID == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductAttributeLocalizedID", DbType.Int32, ProductAttributeLocalizedID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductAttributeLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized product attribute by product attribute id and language id
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product attribute content</returns>
        public override DBProductAttributeLocalized GetProductAttributeLocalizedByProductAttributeIDAndLanguageID(int ProductAttributeID, int LanguageID)
        {
            DBProductAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedLoadByProductAttributeIDAndLanguageID");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductAttributeLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="Description">Description text</param>
        /// <returns>DBProductAttributeLocalized</returns>
        public override DBProductAttributeLocalized InsertProductAttributeLocalized(int ProductAttributeID,
            int LanguageID, string Name, string Description)
        {
            DBProductAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedInsert");
            db.AddOutParameter(dbCommand, "ProductAttributeLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductAttributeLocalizedID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductAttributeLocalizedID"));
                item = GetProductAttributeLocalizedByID(ProductAttributeLocalizedID);
            }
            return item;
        }

        /// <summary>
        /// Update a localized product attribute
        /// </summary>
        /// <param name="ProductAttributeLocalizedID">Localized product attribute identifier</param>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="Description">Description text</param>
        /// <returns>DBProductAttributeLocalized</returns>
        public override DBProductAttributeLocalized UpdateProductAttributeLocalized(int ProductAttributeLocalizedID,
            int ProductAttributeID, int LanguageID, string Name, string Description)
        {
            DBProductAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedUpdate");
            db.AddInParameter(dbCommand, "ProductAttributeLocalizedID", DbType.Int32, ProductAttributeLocalizedID);
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Description", DbType.String, Description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductAttributeLocalizedByID(ProductAttributeLocalizedID);

            return item;
        }

        /// <summary>
        /// Deletes a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        public override void DeleteProductVariantAttribute(int ProductVariantAttributeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_ProductAttribute_MappingDelete");
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, ProductVariantAttributeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product variant attribute mappings by product identifier
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public override DBProductVariantAttributeCollection GetProductVariantAttributesByProductVariantID(int ProductVariantID)
        {
            var result = new DBProductVariantAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_ProductAttribute_MappingLoadByProductVariantID");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantAttributeFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping</returns>
        public override DBProductVariantAttribute GetProductVariantAttributeByID(int ProductVariantAttributeID)
        {
            DBProductVariantAttribute productVariantAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_ProductAttribute_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, ProductVariantAttributeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productVariantAttribute = GetProductVariantAttributeFromReader(dataReader);
                }
            }
            return productVariantAttribute;
        }

        /// <summary>
        /// Inserts a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlTypeID">The attribute control type identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public override DBProductVariantAttribute InsertProductVariantAttribute(int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, int AttributeControlTypeID, int DisplayOrder)
        {
            DBProductVariantAttribute productVariantAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_ProductAttribute_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, TextPrompt);
            db.AddInParameter(dbCommand, "IsRequired", DbType.Boolean, IsRequired);
            db.AddInParameter(dbCommand, "AttributeControlTypeID", DbType.Int32, AttributeControlTypeID);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductVariantAttributeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantAttributeID"));
                productVariantAttribute = GetProductVariantAttributeByID(ProductVariantAttributeID);
            }
            return productVariantAttribute;
        }

        /// <summary>
        /// Updates the product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlTypeID">The attribute control type identifier</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public override DBProductVariantAttribute UpdateProductVariantAttribute(int ProductVariantAttributeID, int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, int AttributeControlTypeID, int DisplayOrder)
        {
            DBProductVariantAttribute productVariantAttribute = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_ProductAttribute_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, ProductVariantAttributeID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, ProductAttributeID);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, TextPrompt);
            db.AddInParameter(dbCommand, "IsRequired", DbType.Boolean, IsRequired);
            db.AddInParameter(dbCommand, "AttributeControlTypeID", DbType.Int32, AttributeControlTypeID);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productVariantAttribute = GetProductVariantAttributeByID(ProductVariantAttributeID);
            return productVariantAttribute;
        }

        /// <summary>
        /// Deletes a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        public override void DeleteProductVariantAttributeValue(int ProductVariantAttributeValueID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueDelete");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, ProductVariantAttributeValueID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public override DBProductVariantAttributeValueCollection GetProductVariantAttributeValues(int ProductVariantAttributeID, int LanguageID)
        {
            var result = new DBProductVariantAttributeValueCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLoadByProductVariantAttributeID");
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, ProductVariantAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantAttributeValueFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product variant attribute value</returns>
        public override DBProductVariantAttributeValue GetProductVariantAttributeValueByID(int ProductVariantAttributeValueID, int LanguageID)
        {
            DBProductVariantAttributeValue productVariantAttributeValue = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, ProductVariantAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    productVariantAttributeValue = GetProductVariantAttributeValueFromReader(dataReader);
                }
            }
            return productVariantAttributeValue;
        }

        /// <summary>
        /// Inserts a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="Name">The product variant attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public override DBProductVariantAttributeValue InsertProductVariantAttributeValue(int ProductVariantAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment, 
            bool IsPreSelected, int DisplayOrder)
        {
            DBProductVariantAttributeValue productVariantAttributeValue = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueInsert");
            db.AddOutParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, ProductVariantAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, PriceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, WeightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, IsPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductVariantAttributeValueID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantAttributeValueID"));
                productVariantAttributeValue = GetProductVariantAttributeValueByID(ProductVariantAttributeValueID, 0);
            }
            return productVariantAttributeValue;
        }

        /// <summary>
        /// Updates the product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">The product variant attribute value identifier</param>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="Name">The product variant attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public override DBProductVariantAttributeValue UpdateProductVariantAttributeValue(int ProductVariantAttributeValueID,
            int ProductVariantAttributeID, string Name, decimal PriceAdjustment,
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder)
        {
            DBProductVariantAttributeValue productVariantAttributeValue = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueUpdate");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, ProductVariantAttributeValueID);
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, ProductVariantAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, PriceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, WeightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, IsPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                productVariantAttributeValue = GetProductVariantAttributeValueByID(ProductVariantAttributeValueID, 0);
            return productVariantAttributeValue;
        }

        /// <summary>
        /// Gets localized product variant attribute value by id
        /// </summary>
        /// <param name="ProductVariantAttributeValueLocalizedID">Localized product variant attribute value identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByID(int ProductVariantAttributeValueLocalizedID)
        {
            DBProductVariantAttributeValueLocalized item = null;
            if (ProductVariantAttributeValueLocalizedID == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueLocalizedID", DbType.Int32, ProductVariantAttributeValueLocalizedID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantAttributeValueLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized product variant attribute value by product variant attribute value id and language id
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIDAndLanguageID(int ProductVariantAttributeValueID, int LanguageID)
        {
            DBProductVariantAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedLoadByProductVariantAttributeValueIDAndLanguageID");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, ProductVariantAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantAttributeValueLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized InsertProductVariantAttributeValueLocalized(int ProductVariantAttributeValueID,
            int LanguageID, string Name)
        {
            DBProductVariantAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedInsert");
            db.AddOutParameter(dbCommand, "ProductVariantAttributeValueLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, ProductVariantAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductVariantAttributeValueLocalizedID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantAttributeValueLocalizedID"));
                item = GetProductVariantAttributeValueLocalizedByID(ProductVariantAttributeValueLocalizedID);
            }
            return item;
        }

        /// <summary>
        /// Update a localized product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueLocalizedID">Localized product variant attribute value identifier</param>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized UpdateProductVariantAttributeValueLocalized(int ProductVariantAttributeValueLocalizedID,
            int ProductVariantAttributeValueID, int LanguageID, string Name)
        {
            DBProductVariantAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedUpdate");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueLocalizedID", DbType.Int32, ProductVariantAttributeValueLocalizedID);
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, ProductVariantAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantAttributeValueLocalizedByID(ProductVariantAttributeValueLocalizedID);

            return item;
        }

        /// <summary>
        /// Deletes a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantAttributeCombinationID">Product variant attribute combination identifier</param>
        public override void DeleteProductVariantAttributeCombination(int ProductVariantAttributeCombinationID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeCombinationDelete");
            db.AddInParameter(dbCommand, "ProductVariantAttributeCombinationID", DbType.Int32, ProductVariantAttributeCombinationID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all product variant attribute combinations
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Product variant attribute combination collection</returns>
        public override DBProductVariantAttributeCombinationCollection GetAllProductVariantAttributeCombinations(int ProductVariantID)
        {
            var result = new DBProductVariantAttributeCombinationCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeCombinationLoadAll");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantAttributeCombinationFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantAttributeCombinationID">Product variant attribute combination identifier</param>
        /// <returns>Product variant attribute combination</returns>
        public override DBProductVariantAttributeCombination GetProductVariantAttributeCombinationByID(int ProductVariantAttributeCombinationID)
        {
            DBProductVariantAttributeCombination item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeCombinationLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantAttributeCombinationID", DbType.Int32, ProductVariantAttributeCombinationID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantAttributeCombinationFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The attributes</param>
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="AllowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <returns>Product variant attribute combination</returns>
        public override DBProductVariantAttributeCombination InsertProductVariantAttributeCombination(int ProductVariantID,
            string AttributesXML,
            int StockQuantity,
            bool AllowOutOfStockOrders)
        {
            DBProductVariantAttributeCombination item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeCombinationInsert");
            db.AddOutParameter(dbCommand, "ProductVariantAttributeCombinationID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, AttributesXML);
            db.AddInParameter(dbCommand, "StockQuantity", DbType.Int32, StockQuantity);
            db.AddInParameter(dbCommand, "AllowOutOfStockOrders", DbType.Int32, AllowOutOfStockOrders);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ProductVariantAttributeCombinationID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantAttributeCombinationID"));
                item = GetProductVariantAttributeCombinationByID(ProductVariantAttributeCombinationID);
            }
            return item;
        }

        /// <summary>
        /// Updates a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantAttributeCombinationID">Product variant attribute combination identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The attributes</param>
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="AllowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <returns>Product variant attribute combination</returns>
        public override DBProductVariantAttributeCombination UpdateProductVariantAttributeCombination(int ProductVariantAttributeCombinationID,
            int ProductVariantID,
            string AttributesXML,
            int StockQuantity,
            bool AllowOutOfStockOrders)
        {
            DBProductVariantAttributeCombination item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeCombinationUpdate");
            db.AddInParameter(dbCommand, "ProductVariantAttributeCombinationID", DbType.Int32, ProductVariantAttributeCombinationID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, AttributesXML);
            db.AddInParameter(dbCommand, "StockQuantity", DbType.Int32, StockQuantity);
            db.AddInParameter(dbCommand, "AllowOutOfStockOrders", DbType.Int32, AllowOutOfStockOrders);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantAttributeCombinationByID(ProductVariantAttributeCombinationID);
            return item;
        }

        #endregion
    }
}
