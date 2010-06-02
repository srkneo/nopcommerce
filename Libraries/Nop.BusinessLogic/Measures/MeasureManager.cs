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
            item.MeasureDimensionId = dbItem.MeasureDimensionId;
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
            item.MeasureWeightId = dbItem.MeasureWeightId;
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
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        public static void DeleteMeasureDimension(int measureDimensionId)
        {
            DBProviderManager<DBMeasureProvider>.Provider.DeleteMeasureDimension(measureDimensionId);
            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREDIMENSIONS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public static MeasureDimension GetMeasureDimensionById(int measureDimensionId)
        {
            if (measureDimensionId == 0)
                return null;

            string key = string.Format(MEASUREDIMENSIONS_BY_ID_KEY, measureDimensionId);
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureDimension)obj2;
            }

            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.GetMeasureDimensionById(measureDimensionId);
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
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>Measure dimension</returns>
        public static MeasureDimension GetMeasureDimensionBySystemKeyword(string systemKeyword)
        {
            if (String.IsNullOrEmpty(systemKeyword))
                return null;

            var measureDimensions = GetAllMeasureDimensions();
            foreach (var measureDimension in measureDimensions)
                if (measureDimension.SystemKeyword.ToLowerInvariant() == systemKeyword.ToLowerInvariant())
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
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public static MeasureDimension InsertMeasureDimension(string name,
            string systemKeyword, decimal ratio, int displayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.InsertMeasureDimension(name,
                systemKeyword, ratio, displayOrder);
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
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public static MeasureDimension UpdateMeasureDimension(int measureDimensionId,
            string name, string systemKeyword, decimal ratio, int displayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.UpdateMeasureDimension(measureDimensionId,
                name, systemKeyword, ratio, displayOrder);
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
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureDimension">Source dimension</param>
        /// <param name="targetMeasureDimension">Target dimension</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertDimension(decimal quantity, 
            MeasureDimension sourceMeasureDimension, MeasureDimension targetMeasureDimension)
        {
            decimal result = quantity;
            if (sourceMeasureDimension.MeasureDimensionId == targetMeasureDimension.MeasureDimensionId)
                return result;
            if (result != decimal.Zero && sourceMeasureDimension.MeasureDimensionId != targetMeasureDimension.MeasureDimensionId)
            {
                result = ConvertToPrimaryMeasureDimension(result, sourceMeasureDimension);
                result = ConvertFromPrimaryMeasureDimension(result, targetMeasureDimension);
            }
            result = Math.Round(result, 2);
            return result;
        }

        /// <summary>
        /// Converts to primary measure dimension
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureDimension">Source dimension</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertToPrimaryMeasureDimension(decimal quantity, 
            MeasureDimension sourceMeasureDimension)
        {
            decimal result = quantity;
            if (result != decimal.Zero && sourceMeasureDimension.MeasureDimensionId != BaseDimensionIn.MeasureDimensionId)
            {
                decimal exchangeRatio = sourceMeasureDimension.Ratio;
                if (exchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for dimension [{0}]", sourceMeasureDimension.Name));
                result = result / exchangeRatio;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary dimension
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="targetMeasureDimension">Target dimension</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertFromPrimaryMeasureDimension(decimal quantity, 
            MeasureDimension targetMeasureDimension)
        {
            decimal result = quantity;
            if (result != decimal.Zero && targetMeasureDimension.MeasureDimensionId != BaseDimensionIn.MeasureDimensionId)
            {
                decimal exchangeRatio = targetMeasureDimension.Ratio;
                if (exchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for dimension [{0}]", targetMeasureDimension.Name));
                result = result * exchangeRatio;
            }
            return result;
        }
        
        #endregion

        #region Weights

        /// <summary>
        /// Deletes measure weight
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        public static void DeleteMeasureWeight(int measureWeightId)
        {
            DBProviderManager<DBMeasureProvider>.Provider.DeleteMeasureWeight(measureWeightId);
            if (MeasureManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(MEASUREWEIGHTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public static MeasureWeight GetMeasureWeightById(int measureWeightId)
        {
            if (measureWeightId == 0)
                return null;

            string key = string.Format(MEASUREWEIGHTS_BY_ID_KEY, measureWeightId);
            object obj2 = NopCache.Get(key);
            if (MeasureManager.CacheEnabled && (obj2 != null))
            {
                return (MeasureWeight)obj2;
            }

            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.GetMeasureWeightById(measureWeightId);
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
        /// <param name="systemKeyword">The system keyword</param>
        /// <returns>Measure weight</returns>
        public static MeasureWeight GetMeasureWeightBySystemKeyword(string systemKeyword)
        {
            if (String.IsNullOrEmpty(systemKeyword))
                return null;

            var measureWeights = GetAllMeasureWeights();
            foreach (var measureWeight in measureWeights)
                if (measureWeight.SystemKeyword.ToLowerInvariant() == systemKeyword.ToLowerInvariant())
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
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public static MeasureWeight InsertMeasureWeight(string name,
            string systemKeyword, decimal ratio, int displayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.InsertMeasureWeight(name,
                systemKeyword, ratio, displayOrder);
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
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public static MeasureWeight UpdateMeasureWeight(int measureWeightId, string name,
            string systemKeyword, decimal ratio, int displayOrder)
        {
            var dbItem = DBProviderManager<DBMeasureProvider>.Provider.UpdateMeasureWeight(measureWeightId,
                name, systemKeyword, ratio, displayOrder);
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
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureWeight">Source weight</param>
        /// <param name="targetMeasureWeight">Target weight</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertWeight(decimal quantity,
            MeasureWeight sourceMeasureWeight, MeasureWeight targetMeasureWeight)
        {
            decimal result = quantity;
            if (sourceMeasureWeight.MeasureWeightId == targetMeasureWeight.MeasureWeightId)
                return result;
            if (result != decimal.Zero && sourceMeasureWeight.MeasureWeightId != targetMeasureWeight.MeasureWeightId)
            {
                result = ConvertToPrimaryMeasureWeight(result, sourceMeasureWeight);
                result = ConvertFromPrimaryMeasureWeight(result, targetMeasureWeight);
            }
            result = Math.Round(result, 2);
            return result;
        }

        /// <summary>
        /// Converts to primary measure weight
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="sourceMeasureWeight">Source weight</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertToPrimaryMeasureWeight(decimal quantity, MeasureWeight sourceMeasureWeight)
        {
            decimal result = quantity;
            if (result != decimal.Zero && sourceMeasureWeight.MeasureWeightId != BaseWeightIn.MeasureWeightId)
            {
                decimal exchangeRatio = sourceMeasureWeight.Ratio;
                if (exchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for weight [{0}]", sourceMeasureWeight.Name));
                result = result / exchangeRatio;
            }
            return result;
        }

        /// <summary>
        /// Converts from primary weight
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <param name="targetMeasureWeight">Target weight</param>
        /// <returns>Converted value</returns>
        public static decimal ConvertFromPrimaryMeasureWeight(decimal quantity, 
            MeasureWeight targetMeasureWeight)
        {
            decimal result = quantity;
            if (result != decimal.Zero && targetMeasureWeight.MeasureWeightId != BaseWeightIn.MeasureWeightId)
            {
                decimal exchangeRatio = targetMeasureWeight.Ratio;
                if (exchangeRatio == decimal.Zero)
                    throw new NopException(string.Format("Exchange ratio not set for weight [{0}]", targetMeasureWeight.Name));
                result = result * exchangeRatio;
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
                return MeasureManager.GetMeasureDimensionById(baseDimensionIn);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Common.BaseDimensionIn", value.MeasureDimensionId.ToString());
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
                return MeasureManager.GetMeasureWeightById(baseWeightIn);
            }
            set
            {
                if (value != null)
                    SettingManager.SetParam("Common.BaseWeightIn", value.MeasureWeightId.ToString());
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
