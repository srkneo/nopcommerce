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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Measures;

namespace NopSolutions.NopCommerce.BusinessLogic.Measures
{
    /// <summary>
    /// Measure dimension manager
    /// </summary>
    public partial class MeasureManager
    {
        #region Constants
        private const string MEASUREDIMENSIONS_ALL_KEY = "Nop.measuredimension.all";
        private const string MEASUREDIMENSIONS_BY_ID_KEY = "Nop.measuredimension.id-{0}";
        private const string MEASUREWEIGHTS_ALL_KEY = "Nop.measureweight.all";
        private const string MEASUREWEIGHTS_BY_ID_KEY = "Nop.measureweight.id-{0}";
        private const string MEASUREDIMENSIONS_PATTERN_KEY = "Nop.measuredimension.";
        private const string MEASUREWEIGHTS_PATTERN_KEY = "Nop.measureweight.";
        #endregion

        #region Utilities
        private static MeasureDimensionCollection DBMapping(DBMeasureDimensionCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new MeasureDimensionCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static MeasureDimension DBMapping(DBMeasureDimension dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new MeasureDimension();
            item.MeasureDimensionID = dbItem.MeasureDimensionID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.Ratio = dbItem.Ratio;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static MeasureWeightCollection DBMapping(DBMeasureWeightCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new MeasureWeightCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static MeasureWeight DBMapping(DBMeasureWeight dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new MeasureWeight();
            item.MeasureWeightID = dbItem.MeasureWeightID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.Ratio = dbItem.Ratio;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods

        #region Dimensions
        /// <summary>
        /// Deletes measure dimension
        /// </summary>
        /// <param name="MeasureDimensionID">Measure dimension identifier</param>
        public static void DeleteMeasureDimension(int MeasureDimensionID)
        {
            DBProviderManager<DBMeasureProvider>.Provider.DeleteMeasureDimension(MeasureDimensionID);
            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREDIMENSIONS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="MeasureDimensionID">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public static MeasureDimension GetMeasureDimensionByID(int MeasureDimensionID)
        {
            if (MeasureDimensionID == 0)
                return null;

            string key = string.Format(MEASUREDIMENSIONS_BY_ID_KEY, MeasureDimensionID);
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureDimension)obj2;
            }

            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.GetMeasureDimensionByID(MeasureDimensionID);
            var measureDimension = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureDimension);
            }
            return measureDimension;
        }

        /// <summary>
        /// Gets a measure dimension by system keyword
        /// </summary>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <returns>Measure dimension</returns>
        public static MeasureDimension GetMeasureDimensionBySystemKeyword(string SystemKeyword)
        {
            if (String.IsNullOrEmpty(SystemKeyword))
                return null;

            var measureDimensions = GetAllMeasureDimensions();
            foreach (var measureDimension in measureDimensions)
                if (measureDimension.SystemKeyword.ToLowerInvariant() == SystemKeyword.ToLowerInvariant())
                    return measureDimension;
            return null;
        }

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        public static MeasureDimensionCollection GetAllMeasureDimensions()
        {
            string key = MEASUREDIMENSIONS_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureDimensionCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBMeasureProvider>.Provider.GetAllMeasureDimensions();
            var measureDimensionCollection = DBMapping(dbCollection);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureDimensionCollection);
            }
            return measureDimensionCollection;
        }

        /// <summary>
        /// Inserts a measure dimension
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Ratio">The ratio</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public static MeasureDimension InsertMeasureDimension(string Name,
            string SystemKeyword, decimal Ratio, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.InsertMeasureDimension(Name,
                SystemKeyword, Ratio, DisplayOrder);
            var measure = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREDIMENSIONS_PATTERN_KEY);
            }
            return measure;
        }

        /// <summary>
        /// Updates the measure dimension
        /// </summary>
        /// <param name="MeasureDimensionID">Measure dimension identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Ratio">The ratio</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public static MeasureDimension UpdateMeasureDimension(int MeasureDimensionID, string Name,
            string SystemKeyword, decimal Ratio, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.UpdateMeasureDimension(MeasureDimensionID,
                Name, SystemKeyword, Ratio, DisplayOrder);
            var measure = DBMapping(dbItem);
            
            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREDIMENSIONS_PATTERN_KEY);
            }
            return measure;
        }

        /// <summary>
        /// Converts dimension
        /// </summary>
        /// <param name="Quantity">Quantity</param>
        /// <param name="SourceMeasureDimension">Source dimension</param>
        /// <param name="TargetMeasureDimension">Target dimension</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertDimension(decimal Quantity, 
            MeasureDimension SourceMeasureDimension, MeasureDimension TargetMeasureDimension)
        {
            decimal result = Quantity;
            if (SourceMeasureDimension.MeasureDimensionID == TargetMeasureDimension.MeasureDimensionID)
                return result;
            if (result != decimal.Zero && SourceMeasureDimension.MeasureDimensionID != TargetMeasureDimension.MeasureDimensionID)
            {
                result = ConvertToPrimaryMeasureDimension(result, SourceMeasureDimension);
                result = ConvertFromPrimaryMeasureDimension(result, TargetMeasureDimension);
            }
            result = Math.Round(result, 2);
            return result;
        }

        /// <summary>
        /// Converts to primary measure dimension
        /// </summary>
        /// <param name="Quantity">Quantity</param>
        /// <param name="SourceMeasureDimension">Source dimension</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertToPrimaryMeasureDimension(decimal Quantity, MeasureDimension SourceMeasureDimension)
        {
            decimal result = Quantity;
            if (result != decimal.Zero && SourceMeasureDimension.MeasureDimensionID != BaseDimensionIn.MeasureDimensionID)
            {
                decimal ExchangeRatio = SourceMeasureDimension.Ratio;
                if (ExchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for dimension [{0}]", SourceMeasureDimension.Name));
                result = result / ExchangeRatio;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary dimension
        /// </summary>
        /// <param name="Quantity">Quantity</param>
        /// <param name="TargetMeasureDimension">Target dimension</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertFromPrimaryMeasureDimension(decimal Quantity, MeasureDimension TargetMeasureDimension)
        {
            decimal result = Quantity;
            if (result != decimal.Zero && TargetMeasureDimension.MeasureDimensionID != BaseDimensionIn.MeasureDimensionID)
            {
                decimal ExchangeRatio = TargetMeasureDimension.Ratio;
                if (ExchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for dimension [{0}]", TargetMeasureDimension.Name));
                result = result * ExchangeRatio;
            }
            return result;
        }
        
        #endregion

        #region Weights

        /// <summary>
        /// Deletes measure weight
        /// </summary>
        /// <param name="MeasureWeightID">Measure weight identifier</param>
        public static void DeleteMeasureWeight(int MeasureWeightID)
        {
            DBProviderManager<DBMeasureProvider>.Provider.DeleteMeasureWeight(MeasureWeightID);
            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREWEIGHTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="MeasureWeightID">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public static MeasureWeight GetMeasureWeightByID(int MeasureWeightID)
        {
            if (MeasureWeightID == 0)
                return null;

            string key = string.Format(MEASUREWEIGHTS_BY_ID_KEY, MeasureWeightID);
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureWeight)obj2;
            }

            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.GetMeasureWeightByID(MeasureWeightID);
            var measureWeight = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureWeight);
            }
            return measureWeight;
        }

        /// <summary>
        /// Gets a measure weight by system keyword
        /// </summary>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <returns>Measure weight</returns>
        public static MeasureWeight GetMeasureWeightBySystemKeyword(string SystemKeyword)
        {
            if (String.IsNullOrEmpty(SystemKeyword))
                return null;

            var measureWeights = GetAllMeasureWeights();
            foreach (var measureWeight in measureWeights)
                if (measureWeight.SystemKeyword.ToLowerInvariant() == SystemKeyword.ToLowerInvariant())
                    return measureWeight;
            return null;
        }

        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        public static MeasureWeightCollection GetAllMeasureWeights()
        {
            string key = MEASUREWEIGHTS_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureWeightCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBMeasureProvider>.Provider.GetAllMeasureWeights();
            var measureWeightCollection = DBMapping(dbCollection);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.Max(key, measureWeightCollection);
            }
            return measureWeightCollection;
        }

        /// <summary>
        /// Inserts a measure weight
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Ratio">The ratio</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public static MeasureWeight InsertMeasureWeight(string Name,
            string SystemKeyword, decimal Ratio, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.InsertMeasureWeight(Name,
                SystemKeyword, Ratio, DisplayOrder);
            var weight = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREWEIGHTS_PATTERN_KEY);
            }
            return weight;
        }

        /// <summary>
        /// Updates the measure weight
        /// </summary>
        /// <param name="MeasureWeightID">Measure weight identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Ratio">The ratio</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public static MeasureWeight UpdateMeasureWeight(int MeasureWeightID, string Name,
            string SystemKeyword, decimal Ratio, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.UpdateMeasureWeight(MeasureWeightID,
                Name, SystemKeyword, Ratio, DisplayOrder);
            var weight = DBMapping(dbItem);

            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREWEIGHTS_PATTERN_KEY);
            }
            return weight;
        }

        /// <summary>
        /// Converts weight
        /// </summary>
        /// <param name="Quantity">Quantity</param>
        /// <param name="SourceMeasureWeight">Source weight</param>
        /// <param name="TargetMeasureWeight">Target weight</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertWeight(decimal Quantity,
            MeasureWeight SourceMeasureWeight, MeasureWeight TargetMeasureWeight)
        {
            decimal result = Quantity;
            if (SourceMeasureWeight.MeasureWeightID == TargetMeasureWeight.MeasureWeightID)
                return result;
            if (result != decimal.Zero && SourceMeasureWeight.MeasureWeightID != TargetMeasureWeight.MeasureWeightID)
            {
                result = ConvertToPrimaryMeasureWeight(result, SourceMeasureWeight);
                result = ConvertFromPrimaryMeasureWeight(result, TargetMeasureWeight);
            }
            result = Math.Round(result, 2);
            return result;
        }

        /// <summary>
        /// Converts to primary measure weight
        /// </summary>
        /// <param name="Quantity">Quantity</param>
        /// <param name="SourceMeasureWeight">Source weight</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertToPrimaryMeasureWeight(decimal Quantity, MeasureWeight SourceMeasureWeight)
        {
            decimal result = Quantity;
            if (result != decimal.Zero && SourceMeasureWeight.MeasureWeightID != BaseWeightIn.MeasureWeightID)
            {
                decimal ExchangeRatio = SourceMeasureWeight.Ratio;
                if (ExchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for weight [{0}]", SourceMeasureWeight.Name));
                result = result / ExchangeRatio;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary weight
        /// </summary>
        /// <param name="Quantity">Quantity</param>
        /// <param name="TargetMeasureWeight">Target weight</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertFromPrimaryMeasureWeight(decimal Quantity, MeasureWeight TargetMeasureWeight)
        {
            decimal result = Quantity;
            if (result != decimal.Zero && TargetMeasureWeight.MeasureWeightID != BaseWeightIn.MeasureWeightID)
            {
                decimal ExchangeRatio = TargetMeasureWeight.Ratio;
                if (ExchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for weight [{0}]", TargetMeasureWeight.Name));
                result = result * ExchangeRatio;
            }
            return result;
        }
        
        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the dimension that will be used as a default
        /// </summary>
        public static MeasureDimension BaseDimensionIn
        {
            get
            {
                int baseDimensionIn = SettingManager.GetSettingValueInteger("Common.BaseDimensionIn");
                return MeasureManager.GetMeasureDimensionByID(baseDimensionIn);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Common.BaseDimensionIn", value.MeasureDimensionID.ToString());
            }
        }
       
        /// <summary>
        /// Gets or sets the weight that will be used as a default
        /// </summary>
        public static MeasureWeight BaseWeightIn
        {
            get
            {
                int baseWeightIn = SettingManager.GetSettingValueInteger("Common.BaseWeightIn");
                return MeasureManager.GetMeasureWeightByID(baseWeightIn);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Common.BaseWeightIn", value.MeasureWeightID.ToString());
            }
        }

        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.MeasureManager.CacheEnabled");
            }
        }
        #endregion
    }
}
