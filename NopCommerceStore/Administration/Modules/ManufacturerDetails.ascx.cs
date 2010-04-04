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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ManufacturerDetailsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SelectTab(this.ManufacturerTabs, this.TabID);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Manufacturer manufacturer = ctrlManufacturerInfo.SaveInfo();
                    ctrlManufacturerSEO.SaveInfo();
                    ctrlManufacturerProducts.SaveInfo();

                    CustomerActivityManager.InsertActivity(
                        "EditManufacturer",
                        GetLocaleResourceString("ActivityLog.EditManufacturer"),
                        manufacturer.Name);

                    Response.Redirect(string.Format("ManufacturerDetails.aspx?ManufacturerID={0}&TabID={1}", manufacturer.ManufacturerID, this.GetActiveTabID(this.ManufacturerTabs)));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID);
                if (manufacturer != null)
                {
                    ManufacturerManager.MarkManufacturerAsDeleted(manufacturer.ManufacturerID);

                    CustomerActivityManager.InsertActivity(
                        "DeleteManufacturer",
                        GetLocaleResourceString("ActivityLog.DeleteManufacturer"),
                        manufacturer.Name);
                }
                Response.Redirect("Manufacturers.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int ManufacturerID
        {
            get
            {
                return CommonHelper.QueryStringInt("ManufacturerID");
            }
        }

        protected string TabID
        {
            get
            {
                return CommonHelper.QueryString("TabID");
            }
        }
    }
}