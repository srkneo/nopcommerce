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
    public partial class SQLCheckoutAttributeProvider : DBCheckoutAttributeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCheckoutAttribute GetCheckoutAttributeFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttribute();
            item.CheckoutAttributeID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TextPrompt = NopSqlDataHelper.GetString(dataReader, "TextPrompt");
            item.IsRequired = NopSqlDataHelper.GetBoolean(dataReader, "IsRequired");
            item.ShippableProductRequired = NopSqlDataHelper.GetBoolean(dataReader, "ShippableProductRequired");
            item.IsTaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "IsTaxExempt");
            item.TaxCategoryID = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            item.AttributeControlTypeID = NopSqlDataHelper.GetInt(dataReader, "AttributeControlTypeID");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttributeLocalized();
            item.CheckoutAttributeLocalizedID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeLocalizedID");
            item.CheckoutAttributeID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeID");
            item.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.TextPrompt = NopSqlDataHelper.GetString(dataReader, "TextPrompt");
            return item;
        }

        private DBCheckoutAttributeValue GetCheckoutAttributeValueFromReader(IDataReader dataReader)
        {
            var item = new DBCheckoutAttributeValue();
            item.CheckoutAttributeValueID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeValueID");
            item.CheckoutAttributeID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeID");
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
            item.CheckoutAttributeValueLocalizedID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeValueLocalizedID");
            item.CheckoutAttributeValueID = NopSqlDataHelper.GetInt(dataReader, "CheckoutAttributeValueID");
            item.LanguageID = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        public override void DeleteCheckoutAttribute(int CheckoutAttributeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeDelete");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="DontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public override DBCheckoutAttributeCollection GetAllCheckoutAttributes(int LanguageID, bool DontLoadShippableProductRequired)
        {
            var result = new DBCheckoutAttributeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "DontLoadShippableProductRequired", DbType.Boolean, DontLoadShippableProductRequired);
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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute</returns>
        public override DBCheckoutAttribute GetCheckoutAttributeByID(int CheckoutAttributeID, int LanguageID)
        {
            DBCheckoutAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
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
        /// <param name="Name">Name</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <param name="IsRequired">Value indicating whether the entity is required</param>
        /// <param name="ShippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="IsTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <param name="AttributeControlTypeID">Attribute control type identifier</param>
        /// <param name="DisplayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public override DBCheckoutAttribute InsertCheckoutAttribute(string Name,
            string TextPrompt, bool IsRequired, bool ShippableProductRequired,
            bool IsTaxExempt, int TaxCategoryID, int AttributeControlTypeID,
            int DisplayOrder)
        {
            DBCheckoutAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, TextPrompt);
            db.AddInParameter(dbCommand, "IsRequired", DbType.Boolean, IsRequired);
            db.AddInParameter(dbCommand, "ShippableProductRequired", DbType.Boolean, ShippableProductRequired);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, IsTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "AttributeControlTypeID", DbType.Int32, AttributeControlTypeID);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CheckoutAttributeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeID"));
                item = GetCheckoutAttributeByID(CheckoutAttributeID, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="Name">Name</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <param name="IsRequired">Value indicating whether the entity is required</param>
        /// <param name="ShippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="IsTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <param name="AttributeControlTypeID">Attribute control type identifier</param>
        /// <param name="DisplayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public override DBCheckoutAttribute UpdateCheckoutAttribute(int CheckoutAttributeID,
            string Name, string TextPrompt, bool IsRequired, bool ShippableProductRequired,
            bool IsTaxExempt, int TaxCategoryID, int AttributeControlTypeID,
            int DisplayOrder)
        {
            DBCheckoutAttribute item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, TextPrompt);
            db.AddInParameter(dbCommand, "IsRequired", DbType.Boolean, IsRequired);
            db.AddInParameter(dbCommand, "ShippableProductRequired", DbType.Boolean, ShippableProductRequired);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, IsTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, TaxCategoryID);
            db.AddInParameter(dbCommand, "AttributeControlTypeID", DbType.Int32, AttributeControlTypeID);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeByID(CheckoutAttributeID, 0);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute by id
        /// </summary>
        /// <param name="CheckoutAttributeLocalizedID">Localized checkout attribute identifier</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedByID(int CheckoutAttributeLocalizedID)
        {
            DBCheckoutAttributeLocalized item = null;
            if (CheckoutAttributeLocalizedID == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeLocalizedID", DbType.Int32, CheckoutAttributeLocalizedID);
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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedByCheckoutAttributeIDAndLanguageID(int CheckoutAttributeID, int LanguageID)
        {
            DBCheckoutAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedLoadByCheckoutAttributeIDAndLanguageID");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized InsertCheckoutAttributeLocalized(int CheckoutAttributeID,
            int LanguageID, string Name, string TextPrompt)
        {
            DBCheckoutAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, TextPrompt);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CheckoutAttributeLocalizedID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeLocalizedID"));
                item = GetCheckoutAttributeLocalizedByID(CheckoutAttributeLocalizedID);
            }
            return item;
        }

        /// <summary>
        /// Update a localized checkout attribute
        /// </summary>
        /// <param name="CheckoutAttributeLocalizedID">Localized checkout attribute identifier</param>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public override DBCheckoutAttributeLocalized UpdateCheckoutAttributeLocalized(int CheckoutAttributeLocalizedID,
            int CheckoutAttributeID, int LanguageID, string Name, string TextPrompt)
        {
            DBCheckoutAttributeLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeLocalizedUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeLocalizedID", DbType.Int32, CheckoutAttributeLocalizedID);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "TextPrompt", DbType.String, TextPrompt);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeLocalizedByID(CheckoutAttributeLocalizedID);

            return item;
        }

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        public override void DeleteCheckoutAttributeValue(int CheckoutAttributeValueID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueDelete");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, CheckoutAttributeValueID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public override DBCheckoutAttributeValueCollection GetCheckoutAttributeValues(int CheckoutAttributeID, int LanguageID)
        {
            var result = new DBCheckoutAttributeValueCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLoadByCheckoutAttributeID");
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
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
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value</returns>
        public override DBCheckoutAttributeValue GetCheckoutAttributeValueByID(int CheckoutAttributeValueID, int LanguageID)
        {
            DBCheckoutAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, CheckoutAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
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
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="Name">The checkout attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public override DBCheckoutAttributeValue InsertCheckoutAttributeValue(int CheckoutAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment,
            bool IsPreSelected, int DisplayOrder)
        {
            DBCheckoutAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, PriceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, WeightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, IsPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CheckoutAttributeValueID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeValueID"));
                item = GetCheckoutAttributeValueByID(CheckoutAttributeValueID, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">The checkout attribute value identifier</param>
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="Name">The checkout attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public override DBCheckoutAttributeValue UpdateCheckoutAttributeValue(int CheckoutAttributeValueID,
            int CheckoutAttributeID, string Name, decimal PriceAdjustment,
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder)
        {
            DBCheckoutAttributeValue item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, CheckoutAttributeValueID);
            db.AddInParameter(dbCommand, "CheckoutAttributeID", DbType.Int32, CheckoutAttributeID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, PriceAdjustment);
            db.AddInParameter(dbCommand, "WeightAdjustment", DbType.Decimal, WeightAdjustment);
            db.AddInParameter(dbCommand, "IsPreSelected", DbType.Boolean, IsPreSelected);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeValueByID(CheckoutAttributeValueID, 0);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute value by id
        /// </summary>
        /// <param name="CheckoutAttributeValueLocalizedID">Localized checkout attribute value identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByID(int CheckoutAttributeValueLocalizedID)
        {
            DBCheckoutAttributeValueLocalized item = null;
            if (CheckoutAttributeValueLocalizedID == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueLocalizedID", DbType.Int32, CheckoutAttributeValueLocalizedID);
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
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIDAndLanguageID(int CheckoutAttributeValueID, int LanguageID)
        {
            DBCheckoutAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedLoadByCheckoutAttributeValueIDAndLanguageID");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, CheckoutAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
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
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized InsertCheckoutAttributeValueLocalized(int CheckoutAttributeValueID,
            int LanguageID, string Name)
        {
            DBCheckoutAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedInsert");
            db.AddOutParameter(dbCommand, "CheckoutAttributeValueLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, CheckoutAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CheckoutAttributeValueLocalizedID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CheckoutAttributeValueLocalizedID"));
                item = GetCheckoutAttributeValueLocalizedByID(CheckoutAttributeValueLocalizedID);
            }
            return item;
        }

        /// <summary>
        /// Update a localized checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueLocalizedID">Localized checkout attribute value identifier</param>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public override DBCheckoutAttributeValueLocalized UpdateCheckoutAttributeValueLocalized(int CheckoutAttributeValueLocalizedID,
            int CheckoutAttributeValueID, int LanguageID, string Name)
        {
            DBCheckoutAttributeValueLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CheckoutAttributeValueLocalizedUpdate");
            db.AddInParameter(dbCommand, "CheckoutAttributeValueLocalizedID", DbType.Int32, CheckoutAttributeValueLocalizedID);
            db.AddInParameter(dbCommand, "CheckoutAttributeValueID", DbType.Int32, CheckoutAttributeValueID);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, LanguageID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCheckoutAttributeValueLocalizedByID(CheckoutAttributeValueLocalizedID);

            return item;
        }

        #endregion
    }
}
