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
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;

namespace NopSolutions.NopCommerce.DataAccess.Products.Specs
{
    /// <summary>
    /// Specification attribute provider for SQL Server
    /// </summary>
    public partial class SqlSpecificationAttributeProvider : DBSpecificationAttributeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        /// <summary>
        /// Maps a data reader to a specification attribute
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Specification attribute</returns>
        private DBSpecificationAttribute GetSpecificationAttributeFromReader(IDataReader dataReader)
        {
            var item = new DBSpecificationAttribute();
            item.SpecificationAttributeId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBSpecificationAttributeLocalized GetSpecificationAttributeLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBSpecificationAttributeLocalized();
            item.SpecificationAttributeLocalizedId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeLocalizedID");
            item.SpecificationAttributeId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return item;
        }

        /// <summary>
        /// Maps a data reader to a specification attribute option
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Specification attribute option</returns>
        private DBSpecificationAttributeOption GetSpecificationAttributeOptionFromReader(IDataReader dataReader)
        {
            var item = new DBSpecificationAttributeOption();
            item.SpecificationAttributeOptionId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            item.SpecificationAttributeId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBSpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBSpecificationAttributeOptionLocalized();
            item.SpecificationAttributeOptionLocalizedId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionLocalizedID");
            item.SpecificationAttributeOptionId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return item;
        }

        /// <summary>
        /// Maps a data reader to a product specification attribute
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Product specification attribute</returns>
        private DBProductSpecificationAttribute GetProductSpecificationAttributeFromReader(IDataReader dataReader)
        {
            var item = new DBProductSpecificationAttribute();
            item.ProductSpecificationAttributeId = NopSqlDataHelper.GetInt(dataReader, "ProductSpecificationAttributeID");
            item.ProductId = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            item.SpecificationAttributeOptionId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            item.AllowFiltering = NopSqlDataHelper.GetBoolean(dataReader, "AllowFiltering");
            item.ShowOnProductPage = NopSqlDataHelper.GetBoolean(dataReader, "ShowOnProductPage");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        /// <summary>
        /// Maps a data reader to a specification attribute option filter
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>Specification attribute option filter</returns>
        private DBSpecificationAttributeOptionFilter GetSpecificationAttributeOptionFilterFromReader(IDataReader dataReader)
        {
            var item = new DBSpecificationAttributeOptionFilter();
            item.SpecificationAttributeId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeID");
            item.SpecificationAttributeName = NopSqlDataHelper.GetString(dataReader, "SpecificationAttributeName");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.SpecificationAttributeOptionId = NopSqlDataHelper.GetInt(dataReader, "SpecificationAttributeOptionID");
            item.SpecificationAttributeOptionName = NopSqlDataHelper.GetString(dataReader, "SpecificationAttributeOptionName");
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

        #region SpecificationAttribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute</returns>
        public override DBSpecificationAttribute GetSpecificationAttributeById(int specificationAttributeId, int languageId)
        {
            DBSpecificationAttribute item = null;
            if (specificationAttributeId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSpecificationAttributeFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute collection</returns>
        public override DBSpecificationAttributeCollection GetSpecificationAttributes(int languageId)
        {
            var result = new DBSpecificationAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetSpecificationAttributeFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        public override void DeleteSpecificationAttribute(int specificationAttributeId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeDelete");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a specification attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public override DBSpecificationAttribute InsertSpecificationAttribute(string name, int displayOrder)
        {
            DBSpecificationAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeInsert");
            db.AddOutParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int specificationAttributeId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SpecificationAttributeID"));
                item = GetSpecificationAttributeById(specificationAttributeId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public override DBSpecificationAttribute UpdateSpecificationAttribute(int specificationAttributeId, string name, int displayOrder)
        {
            DBSpecificationAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeUpdate");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetSpecificationAttributeById(specificationAttributeId, 0);

            return item;
        }

        /// <summary>
        /// Gets localized specification attribute by id
        /// </summary>
        /// <param name="specificationAttributeLocalizedId">Localized specification identifier</param>
        /// <returns>Specification attribute content</returns>
        public override DBSpecificationAttributeLocalized GetSpecificationAttributeLocalizedById(int specificationAttributeLocalizedId)
        {
            DBSpecificationAttributeLocalized item = null;
            if (specificationAttributeLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SpecificationAttributeLocalizedID", DbType.Int32, specificationAttributeLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSpecificationAttributeLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized specification attribute by specification attribute id and language id
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute content</returns>
        public override DBSpecificationAttributeLocalized GetSpecificationAttributeLocalizedBySpecificationAttributeIdAndLanguageId(int specificationAttributeId, int languageId)
        {
            DBSpecificationAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLocalizedLoadBySpecificationAttributeIDAndLanguageID");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSpecificationAttributeLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public override DBSpecificationAttributeLocalized InsertSpecificationAttributeLocalized(int specificationAttributeId,
            int languageId, string name)
        {
            DBSpecificationAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLocalizedInsert");
            db.AddOutParameter(dbCommand, "SpecificationAttributeLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int specificationAttributeLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SpecificationAttributeLocalizedID"));
                item = GetSpecificationAttributeLocalizedById(specificationAttributeLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized specification attribute
        /// </summary>
        /// <param name="specificationAttributeLocalizedId">Localized specification attribute identifier</param>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public override DBSpecificationAttributeLocalized UpdateSpecificationAttributeLocalized(int specificationAttributeLocalizedId,
            int specificationAttributeId, int languageId, string name)
        {
            DBSpecificationAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeLocalizedUpdate");
            db.AddInParameter(dbCommand, "SpecificationAttributeLocalizedID", DbType.Int32, specificationAttributeLocalizedId);
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetSpecificationAttributeLocalizedById(specificationAttributeLocalizedId);

            return item;
        }

        #endregion

        #region SpecificationAttributeOption

        /// <summary>
        /// Gets a specification attribute option collection
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public override DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptions(int languageId)
        {
            var result = new DBSpecificationAttributeOptionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetSpecificationAttributeOptionFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option</returns>
        public override DBSpecificationAttributeOption GetSpecificationAttributeOptionById(int specificationAttributeOptionId, int languageId)
        {
            DBSpecificationAttributeOption item = null;
            if (specificationAttributeOptionId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSpecificationAttributeOptionFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute unique identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public override DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttributeId(int specificationAttributeId, int languageId)
        {
            var result = new DBSpecificationAttributeOptionCollection();
            if (specificationAttributeId == 0)
                return result;

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLoadBySpecificationAttributeID");
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetSpecificationAttributeOptionFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public override DBSpecificationAttributeOption InsertSpecificationAttributeOption(int specificationAttributeId,
            string name, int displayOrder)
        {
            DBSpecificationAttributeOption item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionInsert");
            db.AddOutParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int saoId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SpecificationAttributeOptionID"));
                item = GetSpecificationAttributeOptionById(saoId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public override DBSpecificationAttributeOption UpdateSpecificationAttributeOption(int specificationAttributeOptionId,
            int specificationAttributeId, string name, int displayOrder)
        {
            DBSpecificationAttributeOption item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionUpdate");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "SpecificationAttributeID", DbType.Int32, specificationAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.String, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetSpecificationAttributeOptionById(specificationAttributeOptionId, 0);

            return item;
        }

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        public override void DeleteSpecificationAttributeOption(int specificationAttributeOptionId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionDelete");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets localized specification attribute option by id
        /// </summary>
        /// <param name="specificationAttributeOptionLocalizedId">Localized specification attribute option identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public override DBSpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedById(int specificationAttributeOptionLocalizedId)
        {
            DBSpecificationAttributeOptionLocalized item = null;
            if (specificationAttributeOptionLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionLocalizedID", DbType.Int32, specificationAttributeOptionLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSpecificationAttributeOptionLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized specification attribute option by specification attribute option id and language id
        /// </summary>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public override DBSpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIdAndLanguageId(int specificationAttributeOptionId, int languageId)
        {
            DBSpecificationAttributeOptionLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLocalizedLoadBySpecificationAttributeOptionIDAndLanguageID");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSpecificationAttributeOptionLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public override DBSpecificationAttributeOptionLocalized InsertSpecificationAttributeOptionLocalized(int specificationAttributeOptionId,
            int languageId, string name)
        {
            DBSpecificationAttributeOptionLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLocalizedInsert");
            db.AddOutParameter(dbCommand, "SpecificationAttributeOptionLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int specificationAttributeOptionLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SpecificationAttributeOptionLocalizedID"));
                item = GetSpecificationAttributeOptionLocalizedById(specificationAttributeOptionLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionLocalizedId">Localized specification attribute option identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public override DBSpecificationAttributeOptionLocalized UpdateSpecificationAttributeOptionLocalized(int specificationAttributeOptionLocalizedId,
            int specificationAttributeOptionId, int languageId, string name)
        {
            DBSpecificationAttributeOptionLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionLocalizedUpdate");
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionLocalizedID", DbType.Int32, specificationAttributeOptionLocalizedId);
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetSpecificationAttributeOptionLocalizedById(specificationAttributeOptionLocalizedId);

            return item;
        }

        #endregion

        #region ProductSpecificationAttribute

        /// <summary>
        /// Deletes a product specification attribute mapping
        /// </summary>
        /// <param name="productSpecificationAttributeId">Product specification attribute identifier</param>
        public override void DeleteProductSpecificationAttribute(int productSpecificationAttributeId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingDelete");
            db.AddInParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, productSpecificationAttributeId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a product specification attribute mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="allowFiltering">0 to load attributes with AllowFiltering set to false, 0 to load attributes with AllowFiltering set to true, null to load all attributes</param>
        /// <param name="showOnProductPage">0 to load attributes with ShowOnProductPage set to false, 0 to load attributes with ShowOnProductPage set to true, null to load all attributes</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public override DBProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductId(int productId,
            bool? allowFiltering, bool? showOnProductPage)
        {
            var result = new DBProductSpecificationAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            if (allowFiltering.HasValue)
                db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, allowFiltering.Value);
            else
                db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, null);
            if (showOnProductPage.HasValue)
                db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, showOnProductPage.Value);
            else
                db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductSpecificationAttributeFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product specification attribute mapping 
        /// </summary>
        /// <param name="productSpecificationAttributeId">Product specification attribute mapping identifier</param>
        /// <returns>Product specification attribute mapping</returns>
        public override DBProductSpecificationAttribute GetProductSpecificationAttributeById(int productSpecificationAttributeId)
        {
            DBProductSpecificationAttribute item = null;
            if (productSpecificationAttributeId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, productSpecificationAttributeId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductSpecificationAttributeFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product specification attribute mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public override DBProductSpecificationAttribute InsertProductSpecificationAttribute(int productId, int specificationAttributeOptionId,
                 bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            DBProductSpecificationAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, allowFiltering);
            db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, showOnProductPage);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productSpecificationAttributeId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductSpecificationAttributeID"));
                item = GetProductSpecificationAttributeById(productSpecificationAttributeId);
            }
            return item;
        }

        /// <summary>
        /// Updates the product specification attribute mapping
        /// </summary>
        /// <param name="productSpecificationAttributeId">product specification attribute mapping identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute onn the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public override DBProductSpecificationAttribute UpdateProductSpecificationAttribute(int productSpecificationAttributeId,
               int productId, int specificationAttributeOptionId, bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            DBProductSpecificationAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_SpecificationAttribute_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductSpecificationAttributeID", DbType.Int32, productSpecificationAttributeId);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "SpecificationAttributeOptionID", DbType.Int32, specificationAttributeOptionId);
            db.AddInParameter(dbCommand, "AllowFiltering", DbType.Boolean, allowFiltering);
            db.AddInParameter(dbCommand, "ShowOnProductPage", DbType.Boolean, showOnProductPage);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductSpecificationAttributeById(productSpecificationAttributeId);

            return item;
        }

        #endregion

        #region Specification attribute option filter

        /// <summary>
        /// Gets all specification attribute option filter mapping collection by category id
        /// </summary>
        /// <param name="categoryId">Product category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option filter mapping collection</returns>
        public override DBSpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilterByCategoryId(int categoryId, int languageId)
        {
            var result = new DBSpecificationAttributeOptionFilterCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SpecificationAttributeOptionFilter_LoadByFilter");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetSpecificationAttributeOptionFilterFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        #endregion

        #endregion
    }
}
