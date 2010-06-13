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
    public partial class SqlProductAttributeProvider : DBProductAttributeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBProductAttribute GetProductAttributeFromReader(IDataReader dataReader)
        {
            var item = new DBProductAttribute();
            item.ProductAttributeId = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            return item;
        }

        private DBProductAttributeLocalized GetProductAttributeLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBProductAttributeLocalized();
            item.ProductAttributeLocalizedId = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeLocalizedID");
            item.ProductAttributeId = NopSqlDataHelper.GetInt(dataReader, "ProductAttributeID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            return item;
        }

        private DBProductVariantAttributeValue GetProductVariantAttributeValueFromReader(IDataReader dataReader)
        {
            var item = new DBProductVariantAttributeValue();
            item.ProductVariantAttributeValueId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeValueID");
            item.ProductVariantAttributeId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.PriceAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "PriceAdjustment");
            item.WeightAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "WeightAdjustment");
            item.IsPreSelected = NopSqlDataHelper.GetBoolean(dataReader, "IsPreSelected");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBProductVariantAttributeValueLocalized();
            item.ProductVariantAttributeValueLocalizedId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeValueLocalizedID");
            item.ProductVariantAttributeValueId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantAttributeValueID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
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
        /// Deletes a product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        public override void DeleteProductAttribute(int productAttributeId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeDelete");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, productAttributeId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute collection</returns>
        public override DBProductAttributeCollection GetAllProductAttributes(int languageId)
        {
            var result = new DBProductAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
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
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute </returns>
        public override DBProductAttribute GetProductAttributeById(int productAttributeId, int languageId)
        {
            DBProductAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, productAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductAttributeFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <returns>Product attribute </returns>
        public override DBProductAttribute InsertProductAttribute(string name,
            string description)
        {
            DBProductAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeInsert");
            db.AddOutParameter(dbCommand, "ProductAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productAttributeId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductAttributeID"));
                item = GetProductAttributeById(productAttributeId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <returns>Product attribute </returns>
        public override DBProductAttribute UpdateProductAttribute(int productAttributeId,
            string name, string description)
        {
            DBProductAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeUpdate");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, productAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductAttributeById(productAttributeId, 0);
            return item;
        }

        /// <summary>
        /// Gets localized product attribute by id
        /// </summary>
        /// <param name="productAttributeLocalizedId">Localized product attribute identifier</param>
        /// <returns>Product attribute content</returns>
        public override DBProductAttributeLocalized GetProductAttributeLocalizedById(int productAttributeLocalizedId)
        {
            DBProductAttributeLocalized item = null;
            if (productAttributeLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductAttributeLocalizedID", DbType.Int32, productAttributeLocalizedId);
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
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute content</returns>
        public override DBProductAttributeLocalized GetProductAttributeLocalizedByProductAttributeIdAndLanguageId(int productAttributeId, int languageId)
        {
            DBProductAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedLoadByProductAttributeIDAndLanguageID");
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, productAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
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
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product attribute content</returns>
        public override DBProductAttributeLocalized InsertProductAttributeLocalized(int productAttributeId,
            int languageId, string name, string description)
        {
            DBProductAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedInsert");
            db.AddOutParameter(dbCommand, "ProductAttributeLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, productAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productAttributeLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductAttributeLocalizedID"));
                item = GetProductAttributeLocalizedById(productAttributeLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized product attribute
        /// </summary>
        /// <param name="productAttributeLocalizedId">Localized product attribute identifier</param>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product attribute content</returns>
        public override DBProductAttributeLocalized UpdateProductAttributeLocalized(int productAttributeLocalizedId,
            int productAttributeId, int languageId, string name, string description)
        {
            DBProductAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAttributeLocalizedUpdate");
            db.AddInParameter(dbCommand, "ProductAttributeLocalizedID", DbType.Int32, productAttributeLocalizedId);
            db.AddInParameter(dbCommand, "ProductAttributeID", DbType.Int32, productAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductAttributeLocalizedById(productAttributeLocalizedId);

            return item;
        }

        /// <summary>
        /// Deletes a product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        public override void DeleteProductVariantAttributeValue(int productVariantAttributeValueId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueDelete");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, productVariantAttributeValueId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public override DBProductVariantAttributeValueCollection GetProductVariantAttributeValues(int productVariantAttributeId, int languageId)
        {
            var result = new DBProductVariantAttributeValueCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLoadByProductVariantAttributeID");
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, productVariantAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
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
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant attribute value</returns>
        public override DBProductVariantAttributeValue GetProductVariantAttributeValueById(int productVariantAttributeValueId, int languageId)
        {
            DBProductVariantAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, productVariantAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantAttributeValueFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="name">The product variant attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public override DBProductVariantAttributeValue InsertProductVariantAttributeValue(int productVariantAttributeId,
            string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            DBProductVariantAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueInsert");
            db.AddOutParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, productVariantAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, priceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, weightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, isPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productVariantAttributeValueId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantAttributeValueID"));
                item = GetProductVariantAttributeValueById(productVariantAttributeValueId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">The product variant attribute value identifier</param>
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="name">The product variant attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public override DBProductVariantAttributeValue UpdateProductVariantAttributeValue(int productVariantAttributeValueId,
            int productVariantAttributeId, string name,
            decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            DBProductVariantAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueUpdate");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, productVariantAttributeValueId);
            db.AddInParameter(dbCommand, "ProductVariantAttributeID", DbType.Int32, productVariantAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, priceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, weightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, isPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantAttributeValueById(productVariantAttributeValueId, 0);
            return item;
        }

        /// <summary>
        /// Gets localized product variant attribute value by id
        /// </summary>
        /// <param name="productVariantAttributeValueLocalizedId">Localized product variant attribute value identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedById(int productVariantAttributeValueLocalizedId)
        {
            DBProductVariantAttributeValueLocalized item = null;
            if (productVariantAttributeValueLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueLocalizedID", DbType.Int32, productVariantAttributeValueLocalizedId);
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
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIdAndLanguageId(int productVariantAttributeValueId, int languageId)
        {
            DBProductVariantAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedLoadByProductVariantAttributeValueIDAndLanguageID");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, productVariantAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
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
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized InsertProductVariantAttributeValueLocalized(int productVariantAttributeValueId,
            int languageId, string name)
        {
            DBProductVariantAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedInsert");
            db.AddOutParameter(dbCommand, "ProductVariantAttributeValueLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, productVariantAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productVariantAttributeValueLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantAttributeValueLocalizedID"));
                item = GetProductVariantAttributeValueLocalizedById(productVariantAttributeValueLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueLocalizedId">Localized product variant attribute value identifier</param>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public override DBProductVariantAttributeValueLocalized UpdateProductVariantAttributeValueLocalized(int productVariantAttributeValueLocalizedId,
            int productVariantAttributeValueId, int languageId, string name)
        {
            DBProductVariantAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantAttributeValueLocalizedUpdate");
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueLocalizedID", DbType.Int32, productVariantAttributeValueLocalizedId);
            db.AddInParameter(dbCommand, "ProductVariantAttributeValueID", DbType.Int32, productVariantAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantAttributeValueLocalizedById(productVariantAttributeValueLocalizedId);
            
            return item;
        }

        #endregion
    }
}
