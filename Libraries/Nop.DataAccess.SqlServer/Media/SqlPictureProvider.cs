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

namespace NopSolutions.NopCommerce.DataAccess.Media
{
    /// <summary>
    /// Picture provider for SQL Server
    /// </summary>
    public partial class SqlPictureProvider : DBPictureProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBPicture GetPictureFromReader(IDataReader dataReader)
        {
            var item = new DBPicture();
            item.PictureId = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            item.PictureBinary = NopSqlDataHelper.GetBytes(dataReader, "PictureBinary");
            item.Extension = NopSqlDataHelper.GetString(dataReader, "Extension");
            item.IsNew = NopSqlDataHelper.GetBoolean(dataReader, "IsNew");
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
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public override DBPicture GetPictureById(int pictureId)
        {
            DBPicture item = null;
            if (pictureId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPictureFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        public override void DeletePicture(int pictureId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureDelete");
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="extension">The picture extension</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public override DBPicture InsertPicture(byte[] pictureBinary,
            string extension, bool isNew)
        {
            DBPicture item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureInsert");
            db.AddOutParameter(dbCommand, "PictureID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "PictureBinary", DbType.Binary, pictureBinary);
            db.AddInParameter(dbCommand, "Extension", DbType.String, extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, isNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int pictureId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PictureID"));
                item = GetPictureById(pictureId);
            }
            return item;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="extension">The picture extension</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public override DBPicture UpdatePicture(int pictureId, byte[] pictureBinary,
            string extension, bool isNew)
        {
            DBPicture item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PictureUpdate");
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            db.AddInParameter(dbCommand, "PictureBinary", DbType.Binary, pictureBinary);
            db.AddInParameter(dbCommand, "Extension", DbType.String, extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, isNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetPictureById(pictureId);

            return item;
        }
        #endregion
    }
}
