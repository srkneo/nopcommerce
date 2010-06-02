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

namespace NopSolutions.NopCommerce.DataAccess.Templates
{
    /// <summary>
    /// Template provider for SQL Server
    /// </summary>
    public partial class SqlTemplateProvider : DBTemplateProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        private DBCategoryTemplate GetCategoryTemplateFromReader(IDataReader dataReader)
        {
            var item = new DBCategoryTemplate();
            item.CategoryTemplateId = NopSqlDataHelper.GetInt(dataReader, "CategoryTemplateID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TemplatePath = NopSqlDataHelper.GetString(dataReader, "TemplatePath");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBProductTemplate GetProductTemplateFromReader(IDataReader dataReader)
        {
            var item = new DBProductTemplate();
            item.ProductTemplateId = NopSqlDataHelper.GetInt(dataReader, "ProductTemplateID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TemplatePath = NopSqlDataHelper.GetString(dataReader, "TemplatePath");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBManufacturerTemplate GetManufacturerTemplateFromReader(IDataReader dataReader)
        {
            var item = new DBManufacturerTemplate();
            item.ManufacturerTemplateId = NopSqlDataHelper.GetInt(dataReader, "ManufacturerTemplateID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TemplatePath = NopSqlDataHelper.GetString(dataReader, "TemplatePath");
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
        /// Deletes a category template
        /// </summary>
        /// <param name="categoryTemplateId">Category template identifier</param>
        public override void DeleteCategoryTemplate(int categoryTemplateId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateDelete");
            db.AddInParameter(dbCommand, "CategoryTemplateID", DbType.Int32, categoryTemplateId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category template collection</returns>
        public override DBCategoryTemplateCollection GetAllCategoryTemplates()
        {
            var result = new DBCategoryTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCategoryTemplateFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a category template
        /// </summary>
        /// <param name="categoryTemplateId">Category template identifier</param>
        /// <returns>A category template</returns>
        public override DBCategoryTemplate GetCategoryTemplateById(int categoryTemplateId)
        {
            DBCategoryTemplate item = null;
            if (categoryTemplateId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CategoryTemplateID", DbType.Int32, categoryTemplateId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCategoryTemplateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a category template
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="templatePath">The template path</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public override DBCategoryTemplate InsertCategoryTemplate(string name,
            string templatePath, int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBCategoryTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateInsert");
            db.AddOutParameter(dbCommand, "CategoryTemplateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, templatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int categoryTemplateId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CategoryTemplateID"));
                item = GetCategoryTemplateById(categoryTemplateId);
            }
            return item;
        }

        /// <summary>
        /// Updates the category template
        /// </summary>
        /// <param name="categoryTemplateId">Category template identifier</param>
        /// <param name="name">The name</param>
        /// <param name="templatePath">The template path</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>A category template</returns>
        public override DBCategoryTemplate UpdateCategoryTemplate(int categoryTemplateId,
            string name, string templatePath, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBCategoryTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryTemplateUpdate");
            db.AddInParameter(dbCommand, "CategoryTemplateID", DbType.Int32, categoryTemplateId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, templatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCategoryTemplateById(categoryTemplateId);
            return item;
        }

        /// <summary>
        /// Deletes a manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplateId">Manufacturer template identifier</param>
        public override void DeleteManufacturerTemplate(int manufacturerTemplateId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString); 
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateDelete");
            db.AddInParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, manufacturerTemplateId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all manufacturer templates
        /// </summary>
        /// <returns>Manufacturer template collection</returns>
        public override DBManufacturerTemplateCollection GetAllManufacturerTemplates()
        {
            var result = new DBManufacturerTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetManufacturerTemplateFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplateId">Manufacturer template identifier</param>
        /// <returns>Manufacturer template</returns>
        public override DBManufacturerTemplate GetManufacturerTemplateById(int manufacturerTemplateId)
        {
            DBManufacturerTemplate item = null;
            if (manufacturerTemplateId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, manufacturerTemplateId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetManufacturerTemplateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a manufacturer template
        /// </summary>
        /// <param name="name">The manufacturer template identifier</param>
        /// <param name="templatePath">The template path</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public override DBManufacturerTemplate InsertManufacturerTemplate(string name,
            string templatePath, int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBManufacturerTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateInsert");
            db.AddOutParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, templatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int manufacturerTemplateId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ManufacturerTemplateID"));
                item = GetManufacturerTemplateById(manufacturerTemplateId);
            }
            return item;
        }

        /// <summary>
        /// Updates the manufacturer template
        /// </summary>
        /// <param name="manufacturerTemplateId">Manufacturer template identifer</param>
        /// <param name="name">The manufacturer template identifier</param>
        /// <param name="templatePath">The template path</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer template</returns>
        public override DBManufacturerTemplate UpdateManufacturerTemplate(int manufacturerTemplateId,
            string name, string templatePath, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBManufacturerTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ManufacturerTemplateUpdate");
            db.AddInParameter(dbCommand, "ManufacturerTemplateID", DbType.Int32, manufacturerTemplateId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, templatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetManufacturerTemplateById(manufacturerTemplateId);

            return item;
        }

        /// <summary>
        /// Deletes a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        public override void DeleteProductTemplate(int productTemplateId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateDelete");
            db.AddInParameter(dbCommand, "ProductTemplateID", DbType.Int32, productTemplateId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all product templates
        /// </summary>
        /// <returns>Product template collection</returns>
        public override DBProductTemplateCollection GetAllProductTemplates()
        {
            var result = new DBProductTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductTemplateFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product template
        /// </summary>
        /// <param name="productTemplateId">Product template identifier</param>
        /// <returns>Product template</returns>
        public override DBProductTemplate GetProductTemplateById(int productTemplateId)
        {
            DBProductTemplate item = null;
            if (productTemplateId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductTemplateID", DbType.Int32, productTemplateId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductTemplateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product template
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="templatePath">The template path</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public override DBProductTemplate InsertProductTemplate(string name, string templatePath,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBProductTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateInsert");
            db.AddOutParameter(dbCommand, "ProductTemplateID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, templatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productTemplateId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductTemplateID"));
                item = GetProductTemplateById(productTemplateId);
            }
            return item;
        }

        /// <summary>
        /// Updates the product template
        /// </summary>
        /// <param name="productTemplateId">The product template identifier</param>
        /// <param name="name">The name</param>
        /// <param name="templatePath">The template path</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Product template</returns>
        public override DBProductTemplate UpdateProductTemplate(int productTemplateId,
            string name, string templatePath, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBProductTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTemplateUpdate");
            db.AddInParameter(dbCommand, "ProductTemplateID", DbType.Int32, productTemplateId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TemplatePath", DbType.String, templatePath);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductTemplateById(productTemplateId);

            return item;
        }

        #endregion
    }
}
