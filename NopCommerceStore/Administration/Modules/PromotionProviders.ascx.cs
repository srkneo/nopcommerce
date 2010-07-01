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
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using System.IO;
using NopSolutions.NopCommerce.Froogle;
using NopSolutions.NopCommerce.PriceGrabber;


namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class PromotionProvidersControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SelectTab(PromotionProvidersTabs, TabId);
                FillDropDowns();
                BindData();
            }
        }

        private void BindData()
        {
            cbAllowPublicFroogleAccess.Checked = SettingManager.GetSettingValueBoolean("Froogle.AllowPublicFroogleAccess");
        }

        private void FillDropDowns()
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindJQuery();

            base.OnPreRender(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SettingManager.SetParam("Froogle.AllowPublicFroogleAccess", cbAllowPublicFroogleAccess.Checked.ToString());

                    CustomerActivityManager.InsertActivity("EditPromotionProviders", GetLocaleResourceString("ActivityLog.EditPromotionProviders"));

                    Response.Redirect(string.Format("PromotionProviders.aspx?TabID={0}", GetActiveTabId(PromotionProvidersTabs)));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnFroogleGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = string.Format("froogle_{0}_{1}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
                string filePath = string.Format("{0}files\\froogle\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    FroogleService.GenerateFeed(fs);
                }

                string clickhereStr = string.Format("<a href=\"{0}files/froogle/{1}\" target=\"_blank\">{2}</a>", CommonHelper.GetStoreLocation(false), fileName, GetLocaleResourceString("Admin.Froogle.ClickHere"));
                string result = string.Format(GetLocaleResourceString("Admin.PromotionProviders.Froogle.SuccessResult"), clickhereStr);
                ShowMessage(result);
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnPriceGrabberGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = string.Format("pricegrabber_{0}_{1}.csv", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
                string filePath = string.Format("{0}files\\pricegrabber\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    PriceGrabberService.GenerateFeed(fs);
                }

                string clickhereStr = string.Format("<a href=\"{0}files/pricegrabber/{1}\" target=\"_blank\">{2}</a>", CommonHelper.GetStoreLocation(false), fileName, GetLocaleResourceString("Admin.Froogle.ClickHere"));
                string result = string.Format(GetLocaleResourceString("Admin.PromotionProviders.PriceGrabber.SuccessResult"), clickhereStr);
                ShowMessage(result);
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected string TabId
        {
            get
            {
                return CommonHelper.QueryString("TabId");
            }
        }
    }
}