using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.DataAccess.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NopSolutions.NopCommerce.DataAccess;
using System.Data.Common;
using System.Data;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Security
{
    /// <summary>
    /// Blacklist provider for SQL Server
    /// </summary>
    public partial class SqlBlacklistProvider : DBBlacklistProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        /// <summary>
        /// Gets an IP address from a data reader
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>DBBannedIpAddress</returns>
        private DBBannedIpAddress GetIpAddressFromReader(IDataReader dataReader)
        {
            var item = new DBBannedIpAddress();
            item.BannedIpAddressId = NopSqlDataHelper.GetInt(dataReader, "BannedIpAddressID");
            item.Address = NopSqlDataHelper.GetString(dataReader, "Address");
            item.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        /// <summary>
        /// Gets an IP network from a data reader
        /// </summary>
        /// <param name="dataReader">IDataReader</param>
        /// <returns>DBBannedIpNetwork</returns>
        private DBBannedIpNetwork GetIpNetworkFromReader(IDataReader dataReader)
        {
            var item = new DBBannedIpNetwork();
            item.BannedIpNetworkId = NopSqlDataHelper.GetInt(dataReader, "BannedIpNetworkID");
            item.StartAddress = NopSqlDataHelper.GetString(dataReader, "StartAddress");
            item.EndAddress = NopSqlDataHelper.GetString(dataReader, "EndAddress");
            item.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            item.IpException = NopSqlDataHelper.GetString(dataReader, "IpException");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. 
        /// This method is not intended to be used directly from your code
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
        /// Gets an IP address by its identifier
        /// </summary>
        /// <param name="bannedIpAddressId">IP Address unique identifier</param>
        /// <returns>IP address</returns>
        public override DBBannedIpAddress GetIpAddressById(int bannedIpAddressId)
        {
            DBBannedIpAddress item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BannedIpAddressID", DbType.Int32, bannedIpAddressId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetIpAddressFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all IP addresses
        /// </summary>
        /// <returns>IP address collection</returns>
        public override DBBannedIpAddressCollection GetIpAddressAll()
        {
            var result = new DBBannedIpAddressCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetIpAddressFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts an IP address
        /// </summary>
        /// <param name="address">IP address</param>
        /// <param name="comment">Reason why the IP was banned</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP address</returns>
        public override DBBannedIpAddress InsertBannedIpAddress(string address, 
            string comment, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpAddress item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressInsert");
            db.AddOutParameter(dbCommand, "BannedIpAddressID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Address", DbType.String, address);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ipAddressId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BannedIpAddressID"));
                item = GetIpAddressById(ipAddressId);
            }
            return item;
        }

        /// <summary>
        /// Updates an IP address
        /// </summary>
        /// <param name="bannedIpAddressId">IP address unique identifier</param>
        /// <param name="address">IP address</param>
        /// <param name="comment">Reason why the IP was banned</param>
        /// <param name="createdOn">When the record was last updated</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns></returns>
        public override DBBannedIpAddress UpdateBannedIpAddress(int bannedIpAddressId,
            string address, string comment, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpAddress item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressUpdate");
            db.AddInParameter(dbCommand, "BannedIpAddressID", DbType.Int32, bannedIpAddressId);
            db.AddInParameter(dbCommand, "Address", DbType.String, address);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetIpAddressById(bannedIpAddressId);

            return item;
        }

        /// <summary>
        /// Deletes an IP address
        /// </summary>
        /// <param name="bannedIpAddressId">IP address unique identifier</param>
        public override void DeleteBannedIpAddress(int bannedIpAddressId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpAddressDelete");
            db.AddInParameter(dbCommand, "BannedIpAddressID", DbType.Int32, bannedIpAddressId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an IP network by its Id
        /// </summary>
        /// <param name="bannedIpNetworkId">IP network unique identifier</param>
        /// <returns>IP network</returns>
        public override DBBannedIpNetwork GetIpNetworkById(int bannedIpNetworkId)
        {
            DBBannedIpNetwork item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, bannedIpNetworkId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetIpNetworkFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all IP networks
        /// </summary>
        /// <returns>IP network collection</returns>
        public override DBBannedIpNetworkCollection GetIpNetworkAll()
        {
            var result = new DBBannedIpNetworkCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetIpNetworkFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts an IP network
        /// </summary>
        /// <param name="startAddress">First IP address in the range</param>
        /// <param name="endAddress">Last IP address in the range</param>
        /// <param name="comment">Reason why the IP network was banned</param>
        /// <param name="ipException">Exceptions in the range</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP network</returns>
        public override DBBannedIpNetwork InsertBannedIpNetwork(string startAddress,
            string endAddress, string comment, string ipException,
            DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpNetwork item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkInsert");
            db.AddOutParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "StartAddress", DbType.String, startAddress);
            db.AddInParameter(dbCommand, "EndAddress", DbType.String, endAddress);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "IpException", DbType.String, ipException);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ipNetworkId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BannedIpNetworkID"));
                item = GetIpNetworkById(ipNetworkId);
            }
            return item;
        }

        /// <summary>
        /// Updates an IP network
        /// </summary>
        /// <param name="bannedIpNetworkId">IP network unique identifier</param>
        /// <param name="startAddress">First IP address in the range</param>
        /// <param name="endAddress">Last IP address in the range</param>
        /// <param name="comment">Reason why the IP network was banned</param>
        /// <param name="ipException">Exceptions in the range</param>
        /// <param name="createdOn">When the record was inserted</param>
        /// <param name="updatedOn">When the record was last updated</param>
        /// <returns>IP network</returns>
        public override DBBannedIpNetwork UpdateBannedIpNetwork(int bannedIpNetworkId,
            string startAddress, string endAddress, string comment,
            string ipException, DateTime createdOn, DateTime updatedOn)
        {
            DBBannedIpNetwork item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkUpdate");
            db.AddInParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, bannedIpNetworkId);
            db.AddInParameter(dbCommand, "StartAddress", DbType.String, startAddress);
            db.AddInParameter(dbCommand, "EndAddress", DbType.String, endAddress);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "IpException", DbType.String, ipException);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetIpNetworkById(bannedIpNetworkId);

            return item;
        }

        /// <summary>
        /// Deletes an IP network
        /// </summary>
        /// <param name="bannedIpNetworkId">IP network unique identifier</param>
        public override void DeleteBannedIpNetwork(int bannedIpNetworkId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BannedIpNetworkDelete");
            db.AddInParameter(dbCommand, "BannedIpNetworkID", DbType.Int32, bannedIpNetworkId);
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}
