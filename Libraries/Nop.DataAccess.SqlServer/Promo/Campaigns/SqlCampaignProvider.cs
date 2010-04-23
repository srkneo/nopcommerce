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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Campaigns
{
    /// <summary>
    /// Campaign provider for SQL Server
    /// </summary>
    public partial class SqlCampaignProvider : DBCampaignProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCampaign GetCampaignFromReader(IDataReader dataReader)
        {
            var item = new DBCampaign();
            item.CampaignId = NopSqlDataHelper.GetInt(dataReader, "CampaignID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            item.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
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
        /// Gets a campaign by campaign identifier
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        /// <returns>Message template</returns>
        public override DBCampaign GetCampaignById(int campaignId)
        {
            DBCampaign item = null;
            if (campaignId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CampaignID", DbType.Int32, campaignId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCampaignFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        public override void DeleteCampaign(int campaignId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignDelete");
            db.AddInParameter(dbCommand, "CampaignID", DbType.Int32, campaignId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Campaign collection</returns>
        public override DBCampaignCollection GetAllCampaigns()
        {
            var result = new DBCampaignCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCampaignFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a campaign
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Campaign</returns>
        public override DBCampaign InsertCampaign(string name,
            string subject, string body, DateTime createdOn)
        {
            DBCampaign item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignInsert");
            db.AddOutParameter(dbCommand, "CampaignID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int campaignId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CampaignID"));
                item = GetCampaignById(campaignId);
            }
            return item;
        }

        /// <summary>
        /// Updates the campaign
        /// </summary>
        /// <param name="campaignId">The campaign identifier</param>
        /// <param name="name">The name</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Campaign</returns>
        public override DBCampaign UpdateCampaign(int campaignId,
            string name, string subject, string body, DateTime createdOn)
        {
            DBCampaign item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CampaignUpdate");
            db.AddInParameter(dbCommand, "CampaignID", DbType.Int32, campaignId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCampaignById(campaignId);

            return item;
        }
        #endregion
    }
}

