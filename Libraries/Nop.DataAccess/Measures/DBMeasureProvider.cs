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
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Measures
{
    /// <summary>
    /// Acts as a base class for deriving custom measure provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/MeasureProvider")]
    public abstract partial class DBMeasureProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes measure dimension
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        public abstract void DeleteMeasureDimension(int measureDimensionId);

        /// <summary>
        /// Gets a measure dimension by identifier
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <returns>Measure dimension</returns>
        public abstract DBMeasureDimension GetMeasureDimensionById(int measureDimensionId);

        /// <summary>
        /// Gets all measure dimensions
        /// </summary>
        /// <returns>Measure dimension collection</returns>
        public abstract DBMeasureDimensionCollection GetAllMeasureDimensions();

        /// <summary>
        /// Inserts a measure dimension
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public abstract DBMeasureDimension InsertMeasureDimension(string name, 
            string systemKeyword, decimal ratio, int displayOrder);

        /// <summary>
        /// Updates the measure dimension
        /// </summary>
        /// <param name="measureDimensionId">Measure dimension identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure dimension</returns>
        public abstract DBMeasureDimension UpdateMeasureDimension(int measureDimensionId, 
            string name, string systemKeyword, decimal ratio, int displayOrder);
        
        /// <summary>
        /// Deletes measure weight
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        public abstract void DeleteMeasureWeight(int measureWeightId);

        /// <summary>
        /// Gets a measure weight by identifier
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <returns>Measure weight</returns>
        public abstract DBMeasureWeight GetMeasureWeightById(int measureWeightId);
        
        /// <summary>
        /// Gets all measure weights
        /// </summary>
        /// <returns>Measure weight collection</returns>
        public abstract DBMeasureWeightCollection GetAllMeasureWeights();

        /// <summary>
        /// Inserts a measure weight
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public abstract DBMeasureWeight InsertMeasureWeight(string name,
            string systemKeyword, decimal ratio, int displayOrder);

        /// <summary>
        /// Updates the measure weight
        /// </summary>
        /// <param name="measureWeightId">Measure weight identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="ratio">The ratio</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A measure weight</returns>
        public abstract DBMeasureWeight UpdateMeasureWeight(int measureWeightId, string name,
            string systemKeyword, decimal ratio, int displayOrder);

        #endregion
    }
}
