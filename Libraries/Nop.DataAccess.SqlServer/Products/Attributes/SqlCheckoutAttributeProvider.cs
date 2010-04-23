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
    /// Checkout attribute provider for SQL Server
    /// </summary>
    public partial class SqlCheckoutAttributeProvider : DBCheckoutAttributeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCheckoutAttribute GetCheckoutAttributeFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttribute();
            item.CheckoutAttributeId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TextPrompt = NopSqlDataHelper.GetString(dataReader, "TextPrompt");
            item.IsRequired = NopSqlDataHelper.GetBoolean(dataReader, "IsRequired");
            item.ShippableProductRequired = NopSqlDataHelper.GetBoolean(dataReader, "ShippableProductRequired");
            item.IsTaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "IsTaxExempt");
            item.TaxCategoryId = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            item.AttributeControlTypeId = NopSqlDataHelper.GetInt(dataReader, "AttributeControlTypeID");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttributeLocalized();
            item.CheckoutAttributeLocalizedId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeLocalizedID");
            item.CheckoutAttributeId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TextPrompt = NopSqlDataHelper.GetString(dataReader, "TextPrompt");
            return item;
        }

        private DBCheckoutAttributeValue GetCheckoutAttributeValueFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttributeValue();
            item.CheckoutAttributeValueId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeValueID");
            item.CheckoutAttributeId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.PriceAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "PriceAdjustment");
            item.WeightAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "WeightAdjustment");
            item.IsPreSelected = NopSqlDataHelper.GetBoolean(dataReader, "IsPreSelected");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttributeValueLocalized();
            item.CheckoutAttributeValueLocalizedId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeValueLocalizedID");
            item.CheckoutAttributeValueId = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeValueID");
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
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        public override void DeleteCheckoutAttribute(int checkoutAttributeId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeDelete");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="dontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public override DBCheckoutAttributeCollection GetAllCheckoutAttributes(int languageId, bool dontLoadShippableProductRequired)
        {
            var result = new DBCheckoutAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "DontLoadShippableProductRequired", DbType.Boolean, dontLoadShippableProductRequired);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCheckoutAttributeFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute</returns>
        public override DBCheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId, int languageId)
        {
            DBCheckoutAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCheckoutAttributeFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a checkout attribute
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <param name="isRequired">Value indicating whether the entity is required</param>
        /// <param name="shippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="isTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="attributeControlTypeId">Attribute control type identifier</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public override DBCheckoutAttribute InsertCheckoutAttribute(string name,
            string textPrompt, bool isRequired, bool shippableProductRequired,
            bool isTaxExempt, int taxCategoryId, int attributeControlTypeId,
            int displayOrder)
        {
            DBCheckoutAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, textPrompt);
            db.AddInParameter(dbCommand, "IsRequired", DbType.Boolean, isRequired);
            db.AddInParameter(dbCommand, "ShippableProductRequired", DbType.Boolean, shippableProductRequired);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, isTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            db.AddInParameter(dbCommand, "AttributeControlTypeID", DbType.Int32, attributeControlTypeId);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int checkoutAttributeId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeID"));
                item = GetCheckoutAttributeById(checkoutAttributeId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="name">Name</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <param name="isRequired">Value indicating whether the entity is required</param>
        /// <param name="shippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="isTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="attributeControlTypeId">Attribute control type identifier</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public override DBCheckoutAttribute UpdateCheckoutAttribute(int checkoutAttributeId,
            string name, string textPrompt, bool isRequired, bool shippableProductRequired,
            bool isTaxExempt, int taxCategoryId, int attributeControlTypeId,
            int displayOrder)
        {
            DBCheckoutAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, textPrompt);
            db.AddInParameter(dbCommand, "IsRequired", DbType.Boolean, isRequired);
            db.AddInParameter(dbCommand, "ShippableProductRequired", DbType.Boolean, shippableProductRequired);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, isTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            db.AddInParameter(dbCommand, "AttributeControlTypeID", DbType.Int32, attributeControlTypeId);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeById(checkoutAttributeId, 0);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute by id
        /// </summary>
        /// <param name="checkoutAttributeLocalizedId">Localized checkout attribute identifier</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedById(int checkoutAttributeLocalizedId)
        {
            DBCheckoutAttributeLocalized item = null;
            if (checkoutAttributeLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeLocalizedID", DbType.Int32, checkoutAttributeLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCheckoutAttributeLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute by checkout attribute id and language id
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedByCheckoutAttributeIdAndLanguageId(int checkoutAttributeId, int languageId)
        {
            DBCheckoutAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedLoadByCheckoutAttributeIDAndLanguageID");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCheckoutAttributeLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized InsertCheckoutAttributeLocalized(int checkoutAttributeId,
            int languageId, string name, string textPrompt)
        {
            DBCheckoutAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, textPrompt);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int checkoutAttributeLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeLocalizedID"));
                item = GetCheckoutAttributeLocalizedById(checkoutAttributeLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeLocalizedId">Localized checkout attribute identifier</param>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized UpdateCheckoutAttributeLocalized(int checkoutAttributeLocalizedId,
            int checkoutAttributeId, int languageId, string name, string textPrompt)
        {
            DBCheckoutAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeLocalizedID", DbType.Int32, checkoutAttributeLocalizedId);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, textPrompt);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeLocalizedById(checkoutAttributeLocalizedId);

            return item;
        }

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        public override void DeleteCheckoutAttributeValue(int checkoutAttributeValueId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueDelete");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, checkoutAttributeValueId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public override DBCheckoutAttributeValueCollection GetCheckoutAttributeValues(int checkoutAttributeId, int languageId)
        {
            var result = new DBCheckoutAttributeValueCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLoadByCheckoutAttributeID");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCheckoutAttributeValueFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute value</returns>
        public override DBCheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId, int languageId)
        {
            DBCheckoutAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, checkoutAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCheckoutAttributeValueFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="name">The checkout attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public override DBCheckoutAttributeValue InsertCheckoutAttributeValue(int checkoutAttributeId,
            string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            DBCheckoutAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, priceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, weightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, isPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int checkoutAttributeValueId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeValueID"));
                item = GetCheckoutAttributeValueById(checkoutAttributeValueId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">The checkout attribute value identifier</param>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="name">The checkout attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public override DBCheckoutAttributeValue UpdateCheckoutAttributeValue(int checkoutAttributeValueId,
            int checkoutAttributeId, string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            DBCheckoutAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, checkoutAttributeValueId);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, checkoutAttributeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, priceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, weightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, isPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeValueById(checkoutAttributeValueId, 0);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute value by id
        /// </summary>
        /// <param name="checkoutAttributeValueLocalizedId">Localized checkout attribute value identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedById(int checkoutAttributeValueLocalizedId)
        {
            DBCheckoutAttributeValueLocalized item = null;
            if (checkoutAttributeValueLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueLocalizedID", DbType.Int32, checkoutAttributeValueLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCheckoutAttributeValueLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute value by checkout attribute value id and language id
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIdAndLanguageId(int checkoutAttributeValueId, int languageId)
        {
            DBCheckoutAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedLoadByCheckoutAttributeValueIDAndLanguageID");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, checkoutAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCheckoutAttributeValueLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized InsertCheckoutAttributeValueLocalized(int checkoutAttributeValueId,
            int languageId, string name)
        {
            DBCheckoutAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeValueLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, checkoutAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int checkoutAttributeValueLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeValueLocalizedID"));
                item = GetCheckoutAttributeValueLocalizedById(checkoutAttributeValueLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueLocalizedId">Localized checkout attribute value identifier</param>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized UpdateCheckoutAttributeValueLocalized(int checkoutAttributeValueLocalizedId,
            int checkoutAttributeValueId, int languageId, string name)
        {
            DBCheckoutAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueLocalizedID", DbType.Int32, checkoutAttributeValueLocalizedId);
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, checkoutAttributeValueId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeValueLocalizedById(checkoutAttributeValueLocalizedId);

            return item;
        }

        #endregion
    }
}
