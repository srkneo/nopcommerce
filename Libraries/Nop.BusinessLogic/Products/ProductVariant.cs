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
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;

namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Represents a product variant
    /// </summary>
    public partial class ProductVariant : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the ProductVariant class
        /// </summary>
        public ProductVariant()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the product variant identifier
        /// </summary>
        public int ProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer part number
        /// </summary>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product variant is download
        /// </summary>
        public bool IsDownload { get; set; }

        /// <summary>
        /// Gets or sets the download identifier
        /// </summary>
        public int DownloadID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this downloadable product can be downloaded unlimited number of times
        /// </summary>
        public bool UnlimitedDownloads { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of downloads
        /// </summary>
        public int MaxNumberOfDownloads { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product variant has a sample download file
        /// </summary>
        public bool HasSampleDownload { get; set; }

        /// <summary>
        /// Gets or sets the sample download identifier
        /// </summary>
        public int SampleDownloadID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is ship enabled
        /// </summary>
        public bool IsShipEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is free shipping
        /// </summary>
        public bool IsFreeShipping { get; set; }

        /// <summary>
        /// Gets or sets the additional shipping charge
        /// </summary>
        public decimal AdditionalShippingCharge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product variant is marked as tax exempt
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the tax category identifier
        /// </summary>
        public int TaxCategoryID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to manage inventory
        /// </summary>
        public bool ManageInventory { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the minimum stock quantity
        /// </summary>
        public int MinStockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the low stock activity identifier
        /// </summary>
        public int LowStockActivityID { get; set; }

        /// <summary>
        /// Gets or sets the quantity when admin should be notified
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// Gets or sets the order minimum quantity
        /// </summary>
        public int OrderMinimumQuantity { get; set; }

        /// <summary>
        /// Gets or sets the order maximum quantity
        /// </summary>
        public int OrderMaximumQuantity { get; set; }

        /// <summary>
        /// Gets or sets the warehouse identifier
        /// </summary>
        public int WarehouseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable buy button
        /// </summary>
        public bool DisableBuyButton { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the old price
        /// </summary>
        public decimal OldPrice { get; set; }

        /// <summary>
        /// Gets or sets the weight
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets the length
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Gets or sets the width
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureID { get; set; }

        /// <summary>
        /// Gets or sets the available start date and time
        /// </summary>
        public DateTime? AvailableStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the shipped end date and time
        /// </summary>
        public DateTime? AvailableEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        #endregion 

        #region Custom Properties

        /// <summary>
        /// Gets the warehouse
        /// </summary>
        public Warehouse Warehouse
        {
            get
            {
                return WarehouseManager.GetWarehouseByID(WarehouseId);
            }
        }

        /// <summary>
        /// Gets the log type
        /// </summary>
        public LowStockActivityEnum LowStockActivity
        {
            get
            {
                return (LowStockActivityEnum)LowStockActivityID;
            }
            set
            {
                LowStockActivityID = (int)value;
            }
        }

        /// <summary>
        /// Gets the tax category
        /// </summary>
        public TaxCategory TaxCategory
        {
            get
            {
                return TaxCategoryManager.GetTaxCategoryByID(TaxCategoryID);
            }
        }

        /// <summary>
        /// Gets the product
        /// </summary>
        public Product Product
        {
            get
            {
                return ProductManager.GetProductByID(ProductID);
            }
        }

        /// <summary>
        /// Gets the discounts of the product variant
        /// </summary>
        public DiscountCollection AllDiscounts
        {
            get
            {
                return DiscountManager.GetDiscountsByProductVariantID(ProductVariantID);
            }
        }

        /// <summary>
        /// Gets the full product name
        /// </summary>
        public string FullProductName
        {
            get
            {
                Product product = this.Product;
                if (product != null)
                {
                    if (!String.IsNullOrEmpty(Name))
                        return product.Name + " (" + this.Name + ")";
                    return product.Name;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public Picture Picture
        {
            get
            {
                return PictureManager.GetPictureByID(PictureID);
            }
        }

        /// <summary>
        /// Gets the download
        /// </summary>
        public Download Download
        {
            get
            {
                return DownloadManager.GetDownloadByID(DownloadID);
            }
        }

        /// <summary>
        /// Gets the sample download
        /// </summary>
        public Download SampleDownload
        {
            get
            {
                return DownloadManager.GetDownloadByID(SampleDownloadID);
            }
        }
        
        /// <summary>
        /// Gets the product variant attributes
        /// </summary>
        public ProductVariantAttributeCollection ProductVariantAttributes
        {
            get
            {
                return ProductAttributeManager.GetProductVariantAttributesByProductVariantID(ProductVariantID);
            }
        }

        /// <summary>
        /// Gets the tier prices of the product variant
        /// </summary>
        public TierPriceCollection TierPrices
        {
            get
            {
                return ProductManager.GetTierPricesByProductVariantID(ProductVariantID);
            }
        }

        #endregion
    }
}
