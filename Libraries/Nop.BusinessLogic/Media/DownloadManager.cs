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
using System.IO;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Media;

namespace NopSolutions.NopCommerce.BusinessLogic.Media
{
    /// <summary>
    /// Download manager
    /// </summary>
    public partial class DownloadManager
    {
        #region Utilities
        private static DownloadCollection DBMapping(DBDownloadCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new DownloadCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Download DBMapping(DBDownload dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Download();
            item.DownloadID = dbItem.DownloadID;
            item.UseDownloadURL = dbItem.UseDownloadURL;
            item.DownloadURL = dbItem.DownloadURL;
            item.DownloadBinary = dbItem.DownloadBinary;
            item.ContentType = dbItem.ContentType;
            item.Filename = dbItem.Filename;
            item.Extension = dbItem.Extension;
            item.IsNew = dbItem.IsNew;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a download url for an admin area
        /// </summary>
        /// <param name="download">Download instance</param>
        /// <returns>Download url</returns>
        public static string GetAdminDownloadUrl(Download download)
        {
            if (download == null)
                throw new ArgumentNullException("download");
            string url = CommonHelper.GetStoreAdminLocation() + "GetDownloadAdmin.ashx?DownloadID=" + download.DownloadID;
            return url;
        }

        /// <summary>
        /// Gets a download url for a product variant
        /// </summary>
        /// <param name="orderProductVariant">Order product variant instance</param>
        /// <returns>Download url</returns>
        public static string GetDownloadUrl(OrderProductVariant orderProductVariant)
        {
            if (orderProductVariant == null)
                throw new ArgumentNullException("orderProductVariant");

            string url = string.Empty;
            var productVariant = orderProductVariant.ProductVariant;
            if (productVariant != null && productVariant.IsDownload)
            {
                url = string.Format("{0}GetDownload.ashx?OrderProductVariantGUID={1}", CommonHelper.GetStoreLocation(), orderProductVariant.OrderProductVariantGUID);
            }
            return url;
        }

        /// <summary>
        /// Gets a license download url for a product variant
        /// </summary>
        /// <param name="orderProductVariant">Order product variant instance</param>
        /// <returns>Download url</returns>
        public static string GetLicenseDownloadUrl(OrderProductVariant orderProductVariant)
        {
            if (orderProductVariant == null)
                throw new ArgumentNullException("orderProductVariant");

            string url = string.Empty;
            var productVariant = orderProductVariant.ProductVariant;
            if (productVariant != null && productVariant.IsDownload && orderProductVariant.LicenseDownloadID > 0)
            {
                url = string.Format("{0}GetLicense.ashx?OrderProductVariantGUID={1}", CommonHelper.GetStoreLocation(), orderProductVariant.OrderProductVariantGUID);
            }
            return url;
        }


        /// <summary>
        /// Gets a sample download url for a product variant
        /// </summary>
        /// <param name="productVariant">Product variant instance</param>
        /// <returns>Download url</returns>
        public static string GetSampleDownloadUrl(ProductVariant productVariant)
        {
            if (productVariant == null)
                throw new ArgumentNullException("productVariant");

            string url = string.Empty;
            if (productVariant.IsDownload && productVariant.HasSampleDownload)
            {
                url = CommonHelper.GetStoreLocation() + "GetDownload.ashx?SampleDownloadProductVariantID=" + productVariant.ProductVariantID.ToString();
            }
            return url;
        }

        /// <summary>
        /// Gets a download
        /// </summary>
        /// <param name="DownloadID">Download identifier</param>
        /// <returns>Download</returns>
        public static Download GetDownloadByID(int DownloadID)
        {
            if (DownloadID == 0)
                return null;

            var dbItem = DBProviderManager<DBDownloadProvider>.Provider.GetDownloadByID(DownloadID);
            var download = DBMapping(dbItem);
            return download;
        }

        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="DownloadID">Download identifier</param>
        public static void DeleteDownload(int DownloadID)
        {
            DBProviderManager<DBDownloadProvider>.Provider.DeleteDownload(DownloadID);
        }

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="UseDownloadURL">The value indicating whether DownloadURL property should be used</param>
        /// <param name="DownloadURL">The download URL</param>
        /// <param name="DownloadBinary">The download binary</param>
        /// <param name="ContentType">The mime-type of the download</param>
        /// <param name="Filename">The filename of the download</param>
        /// <param name="Extension">The extension</param>
        /// <param name="IsNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public static Download InsertDownload(bool UseDownloadURL, string DownloadURL,
            byte[] DownloadBinary, string ContentType, string Filename, 
            string Extension, bool IsNew)
        {
            if (DownloadURL == null)
                DownloadURL = string.Empty;
            if (Filename == null)
                Filename = string.Empty;
            if (ContentType == null)
                ContentType = string.Empty;
            if (Extension == null)
                Extension = string.Empty;

            var dbItem = DBProviderManager<DBDownloadProvider>.Provider.InsertDownload(UseDownloadURL,
                DownloadURL, DownloadBinary, ContentType, Filename, Extension, IsNew);
            var download = DBMapping(dbItem);
            return download;
        }

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="DownloadID">The download identifier</param>
        /// <param name="UseDownloadURL">The value indicating whether DownloadURL property should be used</param>
        /// <param name="DownloadURL">The download URL</param>
        /// <param name="DownloadBinary">The download binary</param>
        /// <param name="ContentType">The mime-type of the download</param>
        /// <param name="Filename">The filename of the download</param>
        /// <param name="Extension">The extension</param>
        /// <param name="IsNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public static Download UpdateDownload(int DownloadID, bool UseDownloadURL, string DownloadURL,
            byte[] DownloadBinary, string ContentType, string Filename, 
                string Extension, bool IsNew)
        {
            if (DownloadURL == null)
                DownloadURL = string.Empty;
            if (Filename == null)
                Filename = string.Empty;
            if (ContentType == null)
                ContentType = string.Empty;
            if (Extension == null)
                Extension = string.Empty;

            var dbItem = DBProviderManager<DBDownloadProvider>.Provider.UpdateDownload(DownloadID,
                UseDownloadURL, DownloadURL, DownloadBinary, ContentType, Filename, Extension, IsNew);
            var download = DBMapping(dbItem);
            return download;
        }

        /// <summary>
        /// Gets the download binary array
        /// </summary>
        /// <param name="fs">File stream</param>
        /// <param name="size">Download size</param>
        /// <returns>Download binary array</returns>
        public static byte[] GetDownloadBits(Stream fs, int size)
        {
            byte[] binary = new byte[size];
            fs.Read(binary, 0, size);
            return binary;
        }
        #endregion
    }
}
