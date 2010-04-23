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

namespace NopSolutions.NopCommerce.DataAccess.Measures
{
    /// <summary>
    /// Measure provider for SQL Server
    /// </summary>
    public partial class SqlMeasureProvider : DBMeasureProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBMeasureWeight GetMeasureWeightFromReader(IDataReader dataReader)
        {
            var item = new DBMeasureWeight();
            item.MeasureWeightId = NopSqlDataHelper.GetInt(dataReader, "MeasureWeightID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            item.Ratio = NopSqlDataHelper.GetDecimal(dataReader, "Ratio");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBMeasureDimension GetMeasureDimensionFromReader(IDataReader dataReader)
        {
            var item = new DBMeasureDimension();
            item.MeasureDimensionId = NopSqlDataHelper.GetInt(dataReader, "MeasureDimensionID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            item.Ratio = NopSqlDataHelper.GetDecimal(dataReader, "Ratio");
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
        /// Deletes measure dimension
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        public override void DeleteMeasureDimension(int measureDimensionId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionDelete");
            db.AddInParameter(dbCommand, "MeasureDimensionID", DbType.Int32, measureDimensionId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public override DBMeasureDimension GetMeasureDimensionById(int measureDimensionId)
        {
            DBMeasureDimension measureDimension = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MeasureDimensionID", DbType.Int32, measureDimensionId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    measureDimension = GetMeasureDimensionFromReader(dataReader);
                }
            }
            return measureDimension;
        }

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        public override DBMeasureDimensionCollection GetAllMeasureDimensions()
        {

            var result = new DBMeasureDimensionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetMeasureDimensionFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a measure dimension
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public override DBMeasureDimension InsertMeasureDimension(string name, 
            string systemKeyword, decimal ratio, int displayOrder)
        {
            DBMeasureDimension item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionInsert");
            db.AddOutParameter(dbCommand, "MeasureDimensionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Ratio", DbType.Decimal, ratio);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int measureDimensionId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@MeasureDimensionID"));
                item = GetMeasureDimensionById(measureDimensionId);
            }
            return item;
        }

        /// <summary>
        /// Updates the measure dimension
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public override DBMeasureDimension UpdateMeasureDimension(int measureDimensionId, 
            string name, string systemKeyword, decimal ratio, int displayOrder)
        {
            DBMeasureDimension item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureDimensionUpdate");
            db.AddInParameter(dbCommand, "MeasureDimensionID", DbType.Int32, measureDimensionId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Ratio", DbType.Decimal, ratio);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetMeasureDimensionById(measureDimensionId);

            return item;
        }

        /// <summary>
        /// Deletes measure weight
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        public override void DeleteMeasureWeight(int measureWeightId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightDelete");
            db.AddInParameter(dbCommand, "MeasureWeightID", DbType.Int32, measureWeightId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public override DBMeasureWeight GetMeasureWeightById(int measureWeightId)
        {
            DBMeasureWeight item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MeasureWeightID", DbType.Int32, measureWeightId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetMeasureWeightFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        public override DBMeasureWeightCollection GetAllMeasureWeights()
        {
            var result = new DBMeasureWeightCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetMeasureWeightFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a measure weight
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public override DBMeasureWeight InsertMeasureWeight(string name,
            string systemKeyword, decimal ratio, int displayOrder)
        {
            DBMeasureWeight item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightInsert");
            db.AddOutParameter(dbCommand, "MeasureWeightID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Ratio", DbType.Decimal, ratio);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int measureWeightId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@MeasureWeightID"));
                item = GetMeasureWeightById(measureWeightId);
            }
            return item;
        }

        /// <summary>
        /// Updates the measure weight
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public override DBMeasureWeight UpdateMeasureWeight(int measureWeightId, string name,
            string systemKeyword, decimal ratio, int displayOrder)
        {
            DBMeasureWeight item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MeasureWeightUpdate");
            db.AddInParameter(dbCommand, "MeasureWeightID", DbType.Int32, measureWeightId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Ratio", DbType.Decimal, ratio);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetMeasureWeightById(measureWeightId);

            return item;
        }


        #endregion
    }
}
