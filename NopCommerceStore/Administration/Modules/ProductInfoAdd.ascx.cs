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
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Web.Administration.Modules;
 
namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductInfoAddControl : BaseNopAdministrationUserControl
    {
        private void FillDropDowns()
        {
            CommonHelper.FillDropDownWithEnum(this.ddlDownloadActivationType, typeof(DownloadActivationTypeEnum));

            CommonHelper.FillDropDownWithEnum(this.ddlCyclePeriod, typeof(RecurringProductCyclePeriodEnum));
            
            this.ddlTemplate.Items.Clear();
            ProductTemplateCollection productTemplateCollection = TemplateManager.GetAllProductTemplates();
            foreach (ProductTemplate productTemplate in productTemplateCollection)
            {
                ListItem item2 = new ListItem(productTemplate.Name, productTemplate.ProductTemplateID.ToString());
                this.ddlTemplate.Items.Add(item2);
            }

            this.ddlProductType.Items.Clear();
            ProductTypeCollection productTypeCollection = ProductManager.GetAllProductTypes();
            foreach (ProductType productType in productTypeCollection)
            {
                ListItem item2 = new ListItem(productType.Name, productType.ProductTypeID.ToString());
                this.ddlProductType.Items.Add(item2);
            }

            this.ddlTaxCategory.Items.Clear();
            ListItem itemTaxCategory = new ListItem("---", "0");
            this.ddlTaxCategory.Items.Add(itemTaxCategory);
            TaxCategoryCollection taxCategoryCollection = TaxCategoryManager.GetAllTaxCategories();
            foreach (TaxCategory taxCategory in taxCategoryCollection)
            {
                ListItem item2 = new ListItem(taxCategory.Name, taxCategory.TaxCategoryID.ToString());
                this.ddlTaxCategory.Items.Add(item2);
            }

            this.ddlWarehouse.Items.Clear();
            ListItem itemWarehouse = new ListItem("---", "0");
            this.ddlWarehouse.Items.Add(itemWarehouse);
            WarehouseCollection warehouseCollection = WarehouseManager.GetAllWarehouses();
            foreach (Warehouse warehouse in warehouseCollection)
            {
                ListItem item2 = new ListItem(warehouse.Name, warehouse.WarehouseID.ToString());
                this.ddlWarehouse.Items.Add(item2);
            }

            CommonHelper.FillDropDownWithEnum(this.ddlLowStockActivity, typeof(LowStockActivityEnum));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string jquery = CommonHelper.GetStoreLocation() + "Scripts/jquery-1.4.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jquery, jquery);

            this.cbIsDownload.Attributes.Add("onclick", "toggleDownloadableProduct();");
            this.cbUseDownloadURL.Attributes.Add("onclick", "toggleDownloadableProduct();");
            this.cbUnlimitedDownloads.Attributes.Add("onclick", "toggleDownloadableProduct();");
            this.cbHasSampleDownload.Attributes.Add("onclick", "toggleDownloadableProduct();");
            this.cbUseSampleDownloadURL.Attributes.Add("onclick", "toggleDownloadableProduct();");
            this.cbHasUserAgreement.Attributes.Add("onclick", "toggleDownloadableProduct();");

            this.cbIsRecurring.Attributes.Add("onclick", "toggleRecurring();");

            this.cbIsShipEnabled.Attributes.Add("onclick", "toggleShipping();");

            this.ddlManageStock.Attributes.Add("onchange", "toggleManageStock();");

            base.OnPreRender(e);
        }

        public Product SaveInfo()
        {
            DateTime nowDT = DateTime.Now;

            string name = txtName.Text.Trim();
            string shortDescription = txtShortDescription.Text.Trim();
            string fullDescription = txtFullDescription.Content.Trim();
            string adminComment = txtAdminComment.Text.Trim();
            int productTypeID = int.Parse(this.ddlProductType.SelectedItem.Value);
            int templateID = int.Parse(this.ddlTemplate.SelectedItem.Value);
            bool showOnHomePage = cbShowOnHomePage.Checked;
            bool allowCustomerReviews = cbAllowCustomerReviews.Checked;
            bool allowCustomerRatings = cbAllowCustomerRatings.Checked;
            bool published = cbPublished.Checked;
            string sku = txtSKU.Text.Trim();
            string manufacturerPartNumber = txtManufacturerPartNumber.Text.Trim();
            bool isGiftCard = cbIsGiftCard.Checked;
            bool isDownload = cbIsDownload.Checked;
            int productVariantDownloadID = 0;
            if (isDownload)
            {
                bool useDownloadURL = cbUseDownloadURL.Checked;
                string downloadURL = txtDownloadURL.Text.Trim();
                byte[] productVariantDownloadBinary = null;
                string downloadContentType = string.Empty;
                string downloadFilename = string.Empty;
                string downloadExtension = string.Empty;

                HttpPostedFile productVariantDownloadFile = fuProductVariantDownload.PostedFile;
                if ((productVariantDownloadFile != null) && (!String.IsNullOrEmpty(productVariantDownloadFile.FileName)))
                {
                    productVariantDownloadBinary = DownloadManager.GetDownloadBits(productVariantDownloadFile.InputStream, productVariantDownloadFile.ContentLength);
                    downloadContentType = productVariantDownloadFile.ContentType;
                    downloadFilename = Path.GetFileNameWithoutExtension(productVariantDownloadFile.FileName);
                    downloadExtension = Path.GetExtension(productVariantDownloadFile.FileName);
                }

                Download productVariantDownload = DownloadManager.InsertDownload(useDownloadURL,
                    downloadURL, productVariantDownloadBinary, downloadContentType,
                    downloadFilename, downloadExtension, true);
                productVariantDownloadID = productVariantDownload.DownloadID;
            }

            bool unlimitedDownloads = cbUnlimitedDownloads.Checked;
            int maxNumberOfDownloads = txtMaxNumberOfDownloads.Value;
            int? downloadExpirationDays = null;
            if (!String.IsNullOrEmpty(txtDownloadExpirationDays.Text.Trim()))
                downloadExpirationDays = int.Parse(txtDownloadExpirationDays.Text.Trim());
            DownloadActivationTypeEnum downloadActivationType = (DownloadActivationTypeEnum)Enum.ToObject(typeof(DownloadActivationTypeEnum), int.Parse(this.ddlDownloadActivationType.SelectedItem.Value));
            bool hasUserAgreement = cbHasUserAgreement.Checked;
            string userAgreementText = txtUserAgreementText.Content;

            bool hasSampleDownload = cbHasSampleDownload.Checked;
            int productVariantSampleDownloadID = 0;
            if (hasSampleDownload)
            {
                bool useSampleDownloadURL = cbUseSampleDownloadURL.Checked;
                string sampleDownloadURL = txtSampleDownloadURL.Text.Trim();
                byte[] productVariantSampleDownloadBinary = null;
                string sampleDownloadContentType = string.Empty;
                string sampleDownloadFilename = string.Empty;
                string sampleDownloadExtension = string.Empty;

                HttpPostedFile productVariantSampleDownloadFile = fuProductVariantSampleDownload.PostedFile;
                if ((productVariantSampleDownloadFile != null) && (!String.IsNullOrEmpty(productVariantSampleDownloadFile.FileName)))
                {
                    productVariantSampleDownloadBinary = DownloadManager.GetDownloadBits(productVariantSampleDownloadFile.InputStream, productVariantSampleDownloadFile.ContentLength);
                    sampleDownloadContentType = productVariantSampleDownloadFile.ContentType;
                    sampleDownloadFilename = Path.GetFileNameWithoutExtension(productVariantSampleDownloadFile.FileName);
                    sampleDownloadExtension = Path.GetExtension(productVariantSampleDownloadFile.FileName);
                }

                Download productVariantSampleDownload = DownloadManager.InsertDownload(useSampleDownloadURL,
                    sampleDownloadURL, productVariantSampleDownloadBinary, sampleDownloadContentType,
                    sampleDownloadFilename, sampleDownloadExtension, true);
                productVariantSampleDownloadID = productVariantSampleDownload.DownloadID;
            }

            bool isRecurring = cbIsRecurring.Checked;
            int cycleLength = txtCycleLength.Value;
            RecurringProductCyclePeriodEnum cyclePeriod = (RecurringProductCyclePeriodEnum)Enum.ToObject(typeof(RecurringProductCyclePeriodEnum), int.Parse(this.ddlCyclePeriod.SelectedItem.Value));
            int totalCycles = txtTotalCycles.Value;

            bool isShipEnabled = cbIsShipEnabled.Checked;
            bool isFreeShipping = cbIsFreeShipping.Checked;
            decimal additionalShippingCharge = txtAdditionalShippingCharge.Value;
            bool isTaxExempt = cbIsTaxExempt.Checked;
            int taxCategoryID = int.Parse(this.ddlTaxCategory.SelectedItem.Value);
            int manageStock = Convert.ToInt32(ddlManageStock.SelectedValue);
            int stockQuantity = txtStockQuantity.Value;
            bool displayStockAvailability = cbDisplayStockAvailability.Checked;
            int minStockQuantity = txtMinStockQuantity.Value;
            LowStockActivityEnum lowStockActivity = (LowStockActivityEnum)Enum.ToObject(typeof(LowStockActivityEnum), int.Parse(this.ddlLowStockActivity.SelectedItem.Value));
            int notifyForQuantityBelow = txtNotifyForQuantityBelow.Value;
            bool allowOutOfStockOrders = cbAllowOutOfStockOrders.Checked;
            int orderMinimumQuantity = txtOrderMinimumQuantity.Value;
            int orderMaximumQuantity = txtOrderMaximumQuantity.Value;
            int warehouseID = int.Parse(this.ddlWarehouse.SelectedItem.Value);
            bool disableBuyButton = cbDisableBuyButton.Checked;
            decimal price = txtPrice.Value;
            decimal oldPrice = txtOldPrice.Value;
            decimal productCost = txtProductCost.Value;
            decimal weight = txtWeight.Value;
            decimal length = txtLength.Value;
            decimal width = txtWidth.Value;
            decimal height = txtHeight.Value;
            DateTime? availableStartDateTime = ctrlAvailableStartDateTimePicker.SelectedDate;
            DateTime? availableEndDateTime = ctrlAvailableEndDateTimePicker.SelectedDate;
            if (availableStartDateTime.HasValue)
            {
                availableStartDateTime = DateTime.SpecifyKind(availableStartDateTime.Value, DateTimeKind.Utc);
            }
            if (availableEndDateTime.HasValue)
            {
                availableEndDateTime = DateTime.SpecifyKind(availableEndDateTime.Value, DateTimeKind.Utc);
            }

            Product product = ProductManager.InsertProduct(name, shortDescription, fullDescription,
                adminComment, productTypeID, templateID, showOnHomePage, string.Empty, string.Empty,
                string.Empty, string.Empty, allowCustomerReviews, allowCustomerRatings, 0, 0,
                 published, false, nowDT, nowDT);

            ProductVariant productVariant = ProductManager.InsertProductVariant(product.ProductID,
                 string.Empty, sku, string.Empty, string.Empty, manufacturerPartNumber,
                 isGiftCard, isDownload, productVariantDownloadID, unlimitedDownloads,
                 maxNumberOfDownloads, downloadExpirationDays, downloadActivationType,
                 hasSampleDownload, productVariantSampleDownloadID,
                 hasUserAgreement, userAgreementText,
                 isRecurring, cycleLength, (int)cyclePeriod, totalCycles,
                 isShipEnabled, isFreeShipping, additionalShippingCharge, isTaxExempt,
                 taxCategoryID, manageStock, stockQuantity, displayStockAvailability,
                 minStockQuantity, lowStockActivity, notifyForQuantityBelow, allowOutOfStockOrders,
                 orderMinimumQuantity, orderMaximumQuantity, warehouseID, disableBuyButton,
                 price, oldPrice, productCost, weight, length,
                 width, height, 0, availableStartDateTime, availableEndDateTime,
                 published, false, 1, nowDT, nowDT);

            return product;
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }
    }
}