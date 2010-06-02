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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Campaigns
{
    /// <summary>
    /// Acts as a base class for deriving custom campaign provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CampaignProvider")]
    public abstract partial class DBCampaignProvider : BaseDBProvider   
    {
        #region Methods

        /// <summary>
        /// Gets a campaign by campaign identifier
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        /// <returns>Message template</returns>
        public abstract DBCampaign GetCampaignById(int campaignId);

        /// <summary>
        /// Deletes a campaign
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        public abstract void DeleteCampaign(int campaignId);

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Campaign collection</returns>
        public abstract DBCampaignCollection GetAllCampaigns();

        /// <summary>
        /// Inserts a campaign
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Campaign</returns>
        public abstract DBCampaign InsertCampaign(string name, 
            string subject, string body, DateTime createdOn);

        /// <summary>
        /// Updates the campaign
        /// </summary>
        /// <param name="campaignId">The campaign identifier</param>
        /// <param name="name">The name</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Campaign</returns>
        public abstract DBCampaign UpdateCampaign(int campaignId,
            string name, string subject, string body, DateTime createdOn);

        #endregion
    }
}

