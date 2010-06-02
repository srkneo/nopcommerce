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

namespace NopSolutions.NopCommerce.DataAccess.Manufacturers
{
    /// <summary>
    /// Manufacturer provider for SQL Server
    /// </summary>
    public partial class SqlManufacturerProvider : DBManufacturerProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBManufacturer GetManufacturerFromReader(IDataReader dataReader)
        {
            var item = new DBManufacturer();
            item.ManufacturerId = NopSqlDataHelper.GetInt(dataReader, "ManufacturerID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.TemplateId = NopSqlDataHelper.GetInt(dataReader, "TemplateID");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            item.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            item.PictureId = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            item.PageSize = NopSqlDataHelper.GetInt(dataReader, "PageSize");
            item.PriceRanges = NopSqlDataHelper.GetString(dataReader, "PriceRanges");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBManufacturerLocalized GetManufacturerLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBManufacturerLocalized();
            item.ManufacturerLocalizedId = NopSqlDataHelper.GetInt(dataReader, "ManufacturerLocalizedID");
            item.ManufacturerId = NopSqlDataHelper.GetInt(dataReader, "ManufacturerID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            item.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            return item;
        }

        private DBProductManufacturer GetProductManufacturerFromReader(IDataReader dataReader)
        {
            var item = new DBProductManufacturer();
            item.ProductManufacturerId = NopSqlDataHelper.GetInt(dataReader, "ProductManufacturerID");
            item.ProductId = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            item.ManufacturerId = NopSqlDataHelper.GetInt(dataReader, "ManufacturerID");
            item.IsFeaturedProduct = NopSqlDataHelper.GetBoolean(dataReader, "IsFeaturedProduct");
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
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer collection</returns>
        public override DBManufacturerCollection GetAllManufacturers(bool showHidden, int languageId)
        {
            var result = new DBManufacturerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetManufacturerFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer</returns>
        public override DBManufacturer GetManufacturerById(int manufacturerId, int languageId)
        {
            DBManufacturer item = null;
            if (manufacturerId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetManufacturerFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="pictureId">The parent picture identifier</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="priceRanges">The price ranges</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public override DBManufacturer InsertManufacturer(string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int pictureId, int pageSize, string priceRanges,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBManufacturer item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerInsert");
            db.AddOutParameter(dbCommand, "ManufacturerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, templateId);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, priceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int manufacturerId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ManufacturerID"));
                item = GetManufacturerById(manufacturerId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="pictureId">The parent picture identifier</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="priceRanges">The price ranges</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public override DBManufacturer UpdateManufacturer(int manufacturerId,
            string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int pictureId, int pageSize, string priceRanges,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBManufacturer item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerUpdate");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, templateId);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, priceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetManufacturerById(manufacturerId, 0);

            return item;
        }

        /// <summary>
        /// Gets localized manufacturer by id
        /// </summary>
        /// <param name="manufacturerLocalizedId">Localized manufacturer identifier</param>
        /// <returns>Manufacturer content</returns>
        public override DBManufacturerLocalized GetManufacturerLocalizedById(int manufacturerLocalizedId)
        {
            DBManufacturerLocalized item = null;
            if (manufacturerLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ManufacturerLocalizedID", DbType.Int32, manufacturerLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetManufacturerLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized manufacturer by manufacturer id and language id
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer content</returns>
        public override DBManufacturerLocalized GetManufacturerLocalizedByManufacturerIdAndLanguageId(int manufacturerId, int languageId)
        {
            DBManufacturerLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLocalizedLoadByManufacturerIDAndLanguageID");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetManufacturerLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se name text</param>
        /// <returns>Manufacturer content</returns>
        public override DBManufacturerLocalized InsertManufacturerLocalized(int manufacturerId,
            int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle, string seName)
        {
            DBManufacturerLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLocalizedInsert");
            db.AddOutParameter(dbCommand, "ManufacturerLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int manufacturerLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ManufacturerLocalizedID"));
                item = GetManufacturerLocalizedById(manufacturerLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized manufacturer
        /// </summary>
        /// <param name="manufacturerLocalizedId">Localized manufacturer identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se name text</param>
        /// <returns>Manufacturer content</returns>
        public override DBManufacturerLocalized UpdateManufacturerLocalized(int manufacturerLocalizedId,
            int manufacturerId, int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle, string seName)
        {
            DBManufacturerLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerLocalizedUpdate");
            db.AddInParameter(dbCommand, "ManufacturerLocalizedID", DbType.Int32, manufacturerLocalizedId);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetManufacturerLocalizedById(manufacturerId);

            return item;
        }

        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifer</param>
        public override void DeleteProductManufacturer(int productManufacturerId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingDelete");
            db.AddInParameter(dbCommand, "ProductManufacturerID", DbType.Int32, productManufacturerId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product product manufacturer collection
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer collection</returns>
        public override DBProductManufacturerCollection GetProductManufacturersByManufacturerId(int manufacturerId, bool showHidden)
        {
            var result = new DBProductManufacturerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingLoadByManufacturerID");
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductManufacturerFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public override DBProductManufacturerCollection GetProductManufacturersByProductId(int productId, bool showHidden)
        {
            var result = new DBProductManufacturerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductManufacturerFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public override DBProductManufacturer GetProductManufacturerById(int productManufacturerId)
        {
            DBProductManufacturer item = null;
            if (productManufacturerId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductManufacturerID", DbType.Int32, productManufacturerId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductManufacturerFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public override DBProductManufacturer InsertProductManufacturer(int productId,
            int manufacturerId, bool isFeaturedProduct, int displayOrder)
        {
            DBProductManufacturer item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductManufacturerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, isFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productManufacturerId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductManufacturerID"));
                item = GetProductManufacturerById(productManufacturerId);
            }
            return item;
        }

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public override DBProductManufacturer UpdateProductManufacturer(int productManufacturerId,
            int productId, int manufacturerId, bool isFeaturedProduct, int displayOrder)
        {
            DBProductManufacturer item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Manufacturer_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductManufacturerID", DbType.Int32, productManufacturerId);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, isFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductManufacturerById(productManufacturerId);

            return item;
        }
        
        #endregion
    }
}
