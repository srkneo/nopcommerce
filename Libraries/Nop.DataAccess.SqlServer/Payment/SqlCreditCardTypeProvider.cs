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

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Credit card type provider for SQL Server
    /// </summary>
    public partial class SqlCreditCardTypeProvider : DBCreditCardTypeProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCreditCardType GetCreditCardTypeFromReader(IDataReader dataReader)
        {
            var item = new DBCreditCardType();
            item.CreditCardTypeId = NopSqlDataHelper.GetInt(dataReader, "CreditCardTypeID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
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
        /// Gets a credit card type
        /// </summary>
        /// <param name="creditCardTypeId">Credit card type identifier</param>
        /// <returns>Credit card type</returns>
        public override DBCreditCardType GetCreditCardTypeById(int creditCardTypeId)
        {
            DBCreditCardType item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CreditCardTypeID", DbType.Int32, creditCardTypeId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCreditCardTypeFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all credit card types
        /// </summary>
        /// <returns>Credit card type collection</returns>
        public override DBCreditCardTypeCollection GetAllCreditCardTypes()
        {
            var result = new DBCreditCardTypeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCreditCardTypeFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a credit card type
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public override DBCreditCardType InsertCreditCardType(string name,
            string systemKeyword, int displayOrder, bool deleted)
        {
            DBCreditCardType item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeInsert");
            db.AddOutParameter(dbCommand, "CreditCardTypeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int creditCardTypeId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CreditCardTypeID"));
                item = GetCreditCardTypeById(creditCardTypeId);
            }
            return item;
        }

        /// <summary>
        /// Updates the credit card type
        /// </summary>
        /// <param name="creditCardTypeId">Credit card type identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public override DBCreditCardType UpdateCreditCardType(int creditCardTypeId,
            string name, string systemKeyword, int displayOrder, bool deleted)
        {
            DBCreditCardType item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CreditCardTypeUpdate");
            db.AddInParameter(dbCommand, "CreditCardTypeID", DbType.Int32, creditCardTypeId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCreditCardTypeById(creditCardTypeId);

            return item;
        }
        #endregion
    }
}
