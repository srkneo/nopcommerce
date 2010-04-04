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
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Web.Administration.Modules;


namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductVariantInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                Product product = productVariant.Product;
                if (product == null)
                    Response.Redirect("Products.aspx");
                
                this.pnlProductVariantID.Visible = true;
                this.lblProductVariantID.Text = productVariant.ProductVariantID.ToString();


                this.txtName.Text = productVariant.Name;
                this.txtSKU.Text = productVariant.SKU;
                this.txtDescription.Content = productVariant.Description;
                this.txtAdminComment.Text = productVariant.AdminComment;
                this.txtManufacturerPartNumber.Text = productVariant.ManufacturerPartNumber;

                //gift card
                this.cbIsGiftCard.Checked = productVariant.IsGiftCard;

                //downloable products
                this.cbIsDownload.Checked = productVariant.IsDownload;
                Download productVariantDownload = productVariant.Download;
                if (productVariantDownload != null)
                {
                    this.cbUseDownloadURL.Checked = productVariantDownload.UseDownloadURL;
                    this.txtDownloadURL.Text = productVariantDownload.DownloadURL;

                    if (productVariantDownload.DownloadBinary != null)
                    {
                        btnRemoveProductVariantDownload.Visible = true;
                        hlProductVariantDownload.Visible = true;
                        string adminDownloadUrl = DownloadManager.GetAdminDownloadUrl(productVariantDownload);
                        hlProductVariantDownload.NavigateUrl = adminDownloadUrl;
                    }
                    else
                    {
                        btnRemoveProductVariantDownload.Visible = false;
                        hlProductVariantDownload.Visible = false;
                    }
                }
                else
                {
                    btnRemoveProductVariantDownload.Visible = false;
                    hlProductVariantDownload.Visible = false;
                }

                this.cbUnlimitedDownloads.Checked = productVariant.UnlimitedDownloads;
                this.txtMaxNumberOfDownloads.Value = productVariant.MaxNumberOfDownloads;
                if (productVariant.DownloadExpirationDays.HasValue)
                    this.txtDownloadExpirationDays.Text = productVariant.DownloadExpirationDays.Value.ToString();
                CommonHelper.SelectListItem(this.ddlDownloadActivationType, productVariant.DownloadActivationType);
                this.cbHasUserAgreement.Checked = productVariant.HasUserAgreement;
                if (productVariant.HasUserAgreement)
                {
                    this.txtUserAgreementText.Content = productVariant.UserAgreementText;
                }

                this.cbHasSampleDownload.Checked = productVariant.HasSampleDownload;
                Download productVariantSampleDownload = productVariant.SampleDownload;
                if (productVariantSampleDownload != null)
                {
                    this.cbUseSampleDownloadURL.Checked = productVariantSampleDownload.UseDownloadURL;
                    this.txtSampleDownloadURL.Text = productVariantSampleDownload.DownloadURL;

                    if (productVariantSampleDownload.DownloadBinary != null)
                    {
                        btnRemoveProductVariantSampleDownload.Visible = true;
                        hlProductVariantSampleDownload.Visible = true;
                        string adminSampleDownloadUrl = DownloadManager.GetAdminDownloadUrl(productVariantSampleDownload);
                        hlProductVariantSampleDownload.NavigateUrl = adminSampleDownloadUrl;
                    }
                    else
                    {
                        btnRemoveProductVariantSampleDownload.Visible = false;
                        hlProductVariantSampleDownload.Visible = false;
                    }
                }
                else
                {
                    btnRemoveProductVariantSampleDownload.Visible = false;
                    hlProductVariantSampleDownload.Visible = false;
                }

                //recurring
                this.cbIsRecurring.Checked = productVariant.IsRecurring;
                this.txtCycleLength.Value = productVariant.CycleLength;
                CommonHelper.SelectListItem(this.ddlCyclePeriod, productVariant.CyclePeriod);
                this.txtTotalCycles.Value = productVariant.TotalCycles;

                //shipping
                this.cbIsShipEnabled.Checked = productVariant.IsShipEnabled;
                this.cbIsFreeShipping.Checked = productVariant.IsFreeShipping;
                this.txtAdditionalShippingCharge.Value = productVariant.AdditionalShippingCharge;

                //tax
                this.cbIsTaxExempt.Checked = productVariant.IsTaxExempt;
                CommonHelper.SelectListItem(this.ddlTaxCategory, productVariant.TaxCategoryID);

                //stock management
                CommonHelper.SelectListItem(this.ddlManageStock, productVariant.ManageInventory);
                this.txtStockQuantity.Value = productVariant.StockQuantity;
                this.cbDisplayStockAvailability.Checked = productVariant.DisplayStockAvailability;
                this.txtMinStockQuantity.Value = productVariant.MinStockQuantity;
                CommonHelper.SelectListItem(this.ddlLowStockActivity, productVariant.LowStockActivityID);
                this.txtNotifyForQuantityBelow.Value = productVariant.NotifyAdminForQuantityBelow;
                this.cbAllowOutOfStockOrders.Checked = productVariant.AllowOutOfStockOrders;
                this.txtOrderMinimumQuantity.Value = productVariant.OrderMinimumQuantity;
                this.txtOrderMaximumQuantity.Value = productVariant.OrderMaximumQuantity;

                //other settings
                CommonHelper.SelectListItem(this.ddlWarehouse, productVariant.WarehouseId);
                this.cbDisableBuyButton.Checked = productVariant.DisableBuyButton;
                this.txtPrice.Value = productVariant.Price;
                this.txtOldPrice.Value = productVariant.OldPrice;
                this.txtProductCost.Value = productVariant.ProductCost;
                this.txtWeight.Value = productVariant.Weight;
                this.txtLength.Value = productVariant.Length;
                this.txtWidth.Value = productVariant.Width;
                this.txtHeight.Value = productVariant.Height;

                Picture productVariantPicture = productVariant.Picture;
                btnRemoveProductVariantImage.Visible = productVariantPicture != null;
                string pictureUrl = PictureManager.GetPictureUrl(productVariantPicture, 100);
                this.iProductVariantPicture.Visible = true;
                this.iProductVariantPicture.ImageUrl = pictureUrl;

                if (productVariant.AvailableStartDateTime.HasValue)
                {
                    this.ctrlAvailableStartDateTimePicker.SelectedDate = productVariant.AvailableStartDateTime.Value;
                }
                if (productVariant.AvailableEndDateTime.HasValue)
                {
                    this.ctrlAvailableEndDateTimePicker.SelectedDate = productVariant.AvailableEndDateTime.Value;
                }

                this.cbPublished.Checked = productVariant.Published;
                this.txtDisplayOrder.Value = productVariant.DisplayOrder;
            }
            else
            {
                this.pnlProductVariantID.Visible = false;

                this.btnRemoveProductVariantImage.Visible = false;
                this.iProductVariantPicture.Visible = false;

                btnRemoveProductVariantDownload.Visible = false;
                hlProductVariantDownload.Visible = false;

                btnRemoveProductVariantSampleDownload.Visible = false;
                hlProductVariantSampleDownload.Visible = false;

                Product product = ProductManager.GetProductByID(this.ProductID);
                if (product == null)
                    Response.Redirect("Products.aspx");
            }
        }

        private void FillDropDowns()
        {
            CommonHelper.FillDropDownWithEnum(this.ddlDownloadActivationType, typeof(DownloadActivationTypeEnum));
            
            CommonHelper.FillDropDownWithEnum(this.ddlCyclePeriod, typeof(RecurringProductCyclePeriodEnum));
            
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
                this.BindData();
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

        public ProductVariant SaveInfo()
        {
            DateTime nowDT = DateTime.Now;

            string name = txtName.Text.Trim();
            string sku = txtSKU.Text.Trim();
            string description = txtDescription.Content;
            string adminComment = txtAdminComment.Text.Trim();
            string manufacturerPartNumber = txtManufacturerPartNumber.Text.Trim();
            bool isGiftCard = cbIsGiftCard.Checked;
            bool isDownload = cbIsDownload.Checked;
            bool unlimitedDownloads = cbUnlimitedDownloads.Checked;
            int maxNumberOfDownloads = txtMaxNumberOfDownloads.Value;
            int? downloadExpirationDays = null;
            if (!String.IsNullOrEmpty(txtDownloadExpirationDays.Text.Trim()))
                downloadExpirationDays = int.Parse(txtDownloadExpirationDays.Text.Trim());
            DownloadActivationTypeEnum downloadActivationType = (DownloadActivationTypeEnum)Enum.ToObject(typeof(DownloadActivationTypeEnum), int.Parse(this.ddlDownloadActivationType.SelectedItem.Value));
            bool hasUserAgreement = cbHasUserAgreement.Checked;
            string userAgreementText = txtUserAgreementText.Content;            
            bool hasSampleDownload = cbHasSampleDownload.Checked;
            
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
            if(availableStartDateTime.HasValue)
            {
                availableStartDateTime = DateTime.SpecifyKind(availableStartDateTime.Value, DateTimeKind.Utc);
            }
            if(availableEndDateTime.HasValue)
            {
                availableEndDateTime = DateTime.SpecifyKind(availableEndDateTime.Value, DateTimeKind.Utc);
            }
            bool published = cbPublished.Checked;
            int displayOrder = txtDisplayOrder.Value;

            ProductVariant productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant != null)
            {
                #region Update
                Picture productVariantPicture = productVariant.Picture;
                HttpPostedFile productVariantPictureFile = fuProductVariantPicture.PostedFile;
                if ((productVariantPictureFile != null) && (!String.IsNullOrEmpty(productVariantPictureFile.FileName)))
                {
                    byte[] productVariantPictureBinary = PictureManager.GetPictureBits(productVariantPictureFile.InputStream, productVariantPictureFile.ContentLength);
                    if (productVariantPicture != null)
                        productVariantPicture = PictureManager.UpdatePicture(productVariantPicture.PictureID, productVariantPictureBinary, productVariantPictureFile.ContentType, true);
                    else
                        productVariantPicture = PictureManager.InsertPicture(productVariantPictureBinary, productVariantPictureFile.ContentType, true);
                }
                int productVariantPictureID = 0;
                if (productVariantPicture != null)
                    productVariantPictureID = productVariantPicture.PictureID;

                int productVariantDownloadID = 0;
                if (isDownload)
                {
                    Download productVariantDownload = productVariant.Download;
                    bool useDownloadURL = cbUseDownloadURL.Checked;
                    string downloadURL = txtDownloadURL.Text.Trim();
                    byte[] productVariantDownloadBinary = null;
                    string downloadContentType = string.Empty;
                    string downloadFilename = string.Empty;
                    string downloadExtension = string.Empty;
                    if (productVariantDownload != null)
                    {
                        productVariantDownloadBinary = productVariantDownload.DownloadBinary;
                        downloadContentType = productVariantDownload.ContentType;
                        downloadFilename = productVariantDownload.Filename;
                        downloadExtension = productVariantDownload.Extension;
                    }

                    HttpPostedFile productVariantDownloadFile = fuProductVariantDownload.PostedFile;
                    if ((productVariantDownloadFile != null) && (!String.IsNullOrEmpty(productVariantDownloadFile.FileName)))
                    {
                        productVariantDownloadBinary = DownloadManager.GetDownloadBits(productVariantDownloadFile.InputStream, productVariantDownloadFile.ContentLength);
                        downloadContentType = productVariantDownloadFile.ContentType;
                        downloadFilename = Path.GetFileNameWithoutExtension(productVariantDownloadFile.FileName);
                        downloadExtension = Path.GetExtension(productVariantDownloadFile.FileName);
                    }

                    if (productVariantDownload != null)
                    {
                        productVariantDownload = DownloadManager.UpdateDownload(productVariantDownload.DownloadID, 
                            useDownloadURL, downloadURL, productVariantDownloadBinary,
                            downloadContentType, downloadFilename, downloadExtension, true);
                    }
                    else
                    {
                        productVariantDownload = DownloadManager.InsertDownload(useDownloadURL, 
                            downloadURL, productVariantDownloadBinary, downloadContentType,
                            downloadFilename, downloadExtension, true);
                    }
                    
                    productVariantDownloadID = productVariantDownload.DownloadID;
                }

                int productVariantSampleDownloadID = 0;
                if (hasSampleDownload)
                {
                    Download productVariantSampleDownload = productVariant.SampleDownload;
                    bool useSampleDownloadURL = cbUseSampleDownloadURL.Checked;
                    string sampleDownloadURL = txtSampleDownloadURL.Text.Trim();
                    byte[] productVariantSampleDownloadBinary = null;
                    string sampleDownloadContentType = string.Empty;
                    string sampleDownloadFilename = string.Empty;
                    string sampleDownloadExtension = string.Empty;
                    if (productVariantSampleDownload != null)
                    {
                        productVariantSampleDownloadBinary = productVariantSampleDownload.DownloadBinary;
                        sampleDownloadContentType = productVariantSampleDownload.ContentType;
                        sampleDownloadFilename = productVariantSampleDownload.Filename;
                        sampleDownloadExtension = productVariantSampleDownload.Extension;
                    }

                    HttpPostedFile productVariantSampleDownloadFile = fuProductVariantSampleDownload.PostedFile;
                    if ((productVariantSampleDownloadFile != null) && (!String.IsNullOrEmpty(productVariantSampleDownloadFile.FileName)))
                    {
                        productVariantSampleDownloadBinary = DownloadManager.GetDownloadBits(productVariantSampleDownloadFile.InputStream, productVariantSampleDownloadFile.ContentLength);
                        sampleDownloadContentType = productVariantSampleDownloadFile.ContentType;
                        sampleDownloadFilename = Path.GetFileNameWithoutExtension(productVariantSampleDownloadFile.FileName);
                        sampleDownloadExtension = Path.GetExtension(productVariantSampleDownloadFile.FileName);
                    }

                    if (productVariantSampleDownload != null)
                    {
                        productVariantSampleDownload = DownloadManager.UpdateDownload(productVariantSampleDownload.DownloadID, 
                            useSampleDownloadURL, sampleDownloadURL, productVariantSampleDownloadBinary,
                            sampleDownloadContentType, sampleDownloadFilename, sampleDownloadExtension, true);
                    }
                    else
                    {
                        productVariantSampleDownload = DownloadManager.InsertDownload(useSampleDownloadURL, 
                            sampleDownloadURL, productVariantSampleDownloadBinary,
                            sampleDownloadContentType, sampleDownloadFilename, sampleDownloadExtension, true);
                    }

                    productVariantSampleDownloadID = productVariantSampleDownload.DownloadID;
                }

                productVariant = ProductManager.UpdateProductVariant(ProductVariantID,
                    productVariant.ProductID, name, sku, description, adminComment, manufacturerPartNumber,
                    isGiftCard, isDownload, productVariantDownloadID, unlimitedDownloads,
                    maxNumberOfDownloads, downloadExpirationDays, downloadActivationType,
                    hasSampleDownload, productVariantSampleDownloadID, hasUserAgreement, userAgreementText,
                    isRecurring, cycleLength, (int)cyclePeriod, totalCycles,
                    isShipEnabled, isFreeShipping, additionalShippingCharge,
                    isTaxExempt, taxCategoryID, manageStock, stockQuantity, displayStockAvailability,
                    minStockQuantity, lowStockActivity, notifyForQuantityBelow,
                    allowOutOfStockOrders, orderMinimumQuantity, orderMaximumQuantity,
                    warehouseID, disableBuyButton, price,
                    oldPrice, productCost, weight, length,
                    width, height, productVariantPictureID,
                    availableStartDateTime, availableEndDateTime, published,
                    productVariant.Deleted, displayOrder, productVariant.CreatedOn, nowDT);
                #endregion
            }
            else
            {
                #region Insert
                Product product = ProductManager.GetProductByID(this.ProductID);
                if (product != null)
                {
                    Picture productVariantPicture = null;
                    HttpPostedFile productVariantPictureFile = fuProductVariantPicture.PostedFile;
                    if ((productVariantPictureFile != null) && (!String.IsNullOrEmpty(productVariantPictureFile.FileName)))
                    {
                        byte[] productVariantPictureBinary = PictureManager.GetPictureBits(productVariantPictureFile.InputStream, productVariantPictureFile.ContentLength);
                        productVariantPicture = PictureManager.InsertPicture(productVariantPictureBinary, productVariantPictureFile.ContentType, true);
                    }
                    int productVariantPictureID = 0;
                    if (productVariantPicture != null)
                        productVariantPictureID = productVariantPicture.PictureID;

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

                        Download productVariantDownload = DownloadManager.InsertDownload(useDownloadURL, downloadURL, 
                            productVariantDownloadBinary, downloadContentType, 
                            downloadFilename, downloadExtension, true);
                        productVariantDownloadID = productVariantDownload.DownloadID;
                    }

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
                            sampleDownloadURL, productVariantSampleDownloadBinary,
                            sampleDownloadContentType, sampleDownloadFilename, sampleDownloadExtension, true);
                        productVariantSampleDownloadID = productVariantSampleDownload.DownloadID;
                    }

                    productVariant = ProductManager.InsertProductVariant(product.ProductID,
                         name, sku, description, adminComment, manufacturerPartNumber,
                         isGiftCard, isDownload, productVariantDownloadID, unlimitedDownloads,
                         maxNumberOfDownloads, downloadExpirationDays, downloadActivationType,
                         hasSampleDownload, productVariantSampleDownloadID, 
                         hasUserAgreement, userAgreementText,
                         isRecurring, cycleLength, (int)cyclePeriod, totalCycles,
                         isShipEnabled, isFreeShipping, additionalShippingCharge, isTaxExempt, taxCategoryID,
                         manageStock, stockQuantity, displayStockAvailability,
                         minStockQuantity, lowStockActivity, notifyForQuantityBelow,
                         allowOutOfStockOrders, orderMinimumQuantity, orderMaximumQuantity,
                         warehouseID, disableBuyButton, price, oldPrice, productCost, weight,
                         length, width, height, productVariantPictureID,
                         availableStartDateTime, availableEndDateTime, published,
                         false, displayOrder, nowDT, nowDT);
                }
                else
                {
                    Response.Redirect("Products.aspx");
                }
                #endregion
            }
            return productVariant;
        }

        protected void btnRemoveProductVariantImage_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    PictureManager.DeletePicture(productVariant.PictureID);
                    ProductManager.RemoveProductVariantPicture(productVariant.ProductVariantID);
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnRemoveProductVariantDownload_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    Download download = productVariant.Download;
                    if (download != null)
                    {
                        download = DownloadManager.UpdateDownload(download.DownloadID,
                            download.UseDownloadURL, download.DownloadURL, null, string.Empty, 
                            string.Empty, string.Empty, true); 
                        //DownloadManager.DeleteDownload(productVariant.DownloadID);
                        //ProductManager.RemoveProductVariantDownload(productVariant.ProductVariantID);
                    }
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnRemoveProductVariantSampleDownload_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    Download download = productVariant.SampleDownload;
                    if (download != null)
                    {
                        download = DownloadManager.UpdateDownload(download.DownloadID,
                            download.UseDownloadURL, download.DownloadURL, null, string.Empty,
                            string.Empty, string.Empty, true);
                        //DownloadManager.DeleteDownload(productVariant.DownloadID);
                        //ProductManager.RemoveProductVariantDownload(productVariant.ProductVariantID);
                    }
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }
        
        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        public int ProductVariantID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductVariantID");
            }
        }
    }
}