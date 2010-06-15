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
    }
}
