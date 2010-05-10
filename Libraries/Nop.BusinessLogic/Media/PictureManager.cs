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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Media;

namespace NopSolutions.NopCommerce.BusinessLogic.Media
{
    /// <summary>
    /// Picture manager
    /// </summary>
    public static partial class PictureManager
    {
        #region Fields
        private static object s_lock;
        #endregion

        #region Ctor
        static PictureManager()
        {
            s_lock = new object();
        }
        #endregion

        #region Utilities
        private static PictureCollection DBMapping(DBPictureCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new PictureCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Picture DBMapping(DBPicture dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Picture();
            item.PictureId = dbItem.PictureId;
            item.PictureBinary = dbItem.PictureBinary;
            item.Extension = dbItem.Extension;
            item.IsNew = dbItem.IsNew;

            return item;
        }
        
        /// <summary>
        /// Returns the first ImageCodeInfo instance with the specified mime type. Some people try to get the ImageCodeInfo instance by index - sounds rather fragile to me.
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo getImageCodeInfo(string mimeType)
        {
            var info = ImageCodecInfo.GetImageEncoders();
            foreach (var ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase)) 
                    return ici;
            return null;
        }

        private static void SavePictureInFile(int PictureId, byte[] pictureBinary, string extension)
        {
            string[] parts = extension.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch(lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            string localFilename = string.Empty;
            localFilename = string.Format("{0}_0.{1}", PictureId.ToString("0000000"), lastPart);
            if(!File.Exists(Path.Combine(LocalImagePath, localFilename)) && !System.IO.Directory.Exists(LocalImagePath))
            {
                System.IO.Directory.CreateDirectory(LocalImagePath);
            }
            File.WriteAllBytes(Path.Combine(LocalImagePath, localFilename), pictureBinary);
        }

        private static byte[] LoadPictureFromFile(int pictureId, string extension)
        {
            string[] parts = extension.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch(lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            string localFilename = string.Empty;
            localFilename = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            if(!File.Exists(Path.Combine(LocalImagePath, localFilename)))
            {
                return new byte[0];
            }
            return File.ReadAllBytes(Path.Combine(LocalImagePath, localFilename));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="imageId">Picture identifier</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(int imageId)
        {
            Picture picture = GetPictureById(imageId);
            return GetPictureUrl(picture);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(Picture picture)
        {
            return GetPictureUrl(picture, 0);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="imageId">Picture identifier</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(int imageId, int targetSize)
        {
            var picture = GetPictureById(imageId);
            return GetPictureUrl(picture, targetSize);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(Picture picture, int targetSize)
        {
            return GetPictureUrl(picture, targetSize, true);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="imageId">Picture identifier</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public static string GetPictureUrl(int imageId, int targetSize, 
            bool showDefaultPicture)
        {
            var picture = GetPictureById(imageId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture);
        }
        
        /// <summary>
        /// Gets all picture urls as a string array
        /// </summary>
        /// <param name="pictureId">Id of picture</param>
        /// <returns>Array containing urls for a picture in all sizes avaliable</returns>
        public static List<String> GetPictureUrls(int pictureId)
        {
            string filter = string.Format("*{0}*.*", pictureId.ToString("0000000"));

            List<String> urls = new List<string>();

            string orginalURL = GetPictureUrl(pictureId);

            string[] currentFiles = System.IO.Directory.GetFiles(PictureManager.LocalThumbImagePath, filter);

            foreach (string currentFileName in currentFiles)
            {
                string url = CommonHelper.GetStoreLocation() + "images/thumbs/" + Path.GetFileName(currentFileName);

                if (url != orginalURL)
                    urls.Add(url);
            }
            
            //add original picture to array
            urls.Add(orginalURL);

            if (urls.Count > 0)
            {
                //reverse sort order (this way the biggest picture usally comes first..)
                urls.Reverse();
            }

            return urls;
        }

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static string GetDefaultPictureUrl(int targetSize)
        {
            return GetDefaultPictureUrl(PictureTypeEnum.Entity, targetSize);
        }

        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static string GetDefaultPictureUrl(PictureTypeEnum defaultPictureType, 
            int targetSize)
        {
            string defaultImageName = string.Empty;
            switch (defaultPictureType)
            {
                case PictureTypeEnum.Entity:
                    defaultImageName = SettingManager.GetSettingValue("Media.DefaultImageName");
                    break;
                case PictureTypeEnum.Avatar:
                    defaultImageName = SettingManager.GetSettingValue("Media.Customer.DefaultAvatarImageName");
                    break;
                default:
                    defaultImageName = SettingManager.GetSettingValue("Media.DefaultImageName");
                    break;
            }


            string relPath = CommonHelper.GetStoreLocation() + "images/" + defaultImageName;
            if (targetSize == 0)
                return relPath;
            else
            {
                string filePath = Path.Combine(LocalImagePath, defaultImageName);
                if (File.Exists(filePath))
                {
                    string fname = string.Format("{0}_{1}{2}",
                        Path.GetFileNameWithoutExtension(filePath),
                        targetSize,
                        Path.GetExtension(filePath));
                    if (!File.Exists(Path.Combine(LocalThumbImagePath, fname)))
                    {
                        var b = new Bitmap(filePath);

                        var newSize = CalculateDimensions(b.Size, targetSize);

                        if (newSize.Width < 1)
                            newSize.Width = 1;
                        if (newSize.Height < 1)
                            newSize.Height = 1;

                        var newBitMap = new Bitmap(newSize.Width, newSize.Height);
                        var g = Graphics.FromImage(newBitMap);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                        var ep = new EncoderParameters();
                        ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureManager.ImageQuality);
                        newBitMap.Save(Path.Combine(LocalThumbImagePath, fname), getImageCodeInfo("image/jpeg"), ep);
                        newBitMap.Dispose();
                        b.Dispose();
                    }
                    return CommonHelper.GetStoreLocation() + "images/thumbs/" + fname;
                }
                return relPath;
            }
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public static string GetPictureUrl(Picture picture, int targetSize,
            bool showDefaultPicture)
        {
            string url = string.Empty;
            if (picture == null || picture.PictureBinary.Length == 0)
            {
                if(showDefaultPicture)
                {
                    url = GetDefaultPictureUrl(targetSize);
                }
                return url;
            }

            string[] parts = picture.Extension.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }

            string localFilename = string.Empty;
            if (picture.IsNew)
            {
                string filter = string.Format("{0}*.*", picture.PictureId.ToString("0000000"));
                string[] currentFiles = System.IO.Directory.GetFiles(PictureManager.LocalThumbImagePath, filter);
                foreach (string currentFileName in currentFiles)
                    File.Delete(Path.Combine(PictureManager.LocalThumbImagePath, currentFileName));

                picture = PictureManager.UpdatePicture(picture.PictureId, picture.PictureBinary, picture.Extension, false);
            }
            lock (s_lock)
            {
                if (targetSize == 0)
                {
                    localFilename = string.Format("{0}.{1}", picture.PictureId.ToString("0000000"), lastPart);
                    if (!File.Exists(Path.Combine(PictureManager.LocalThumbImagePath, localFilename)))
                    {
                        if (!System.IO.Directory.Exists(PictureManager.LocalThumbImagePath))
                        {
                            System.IO.Directory.CreateDirectory(PictureManager.LocalThumbImagePath);
                        }
                        File.WriteAllBytes(Path.Combine(PictureManager.LocalThumbImagePath, localFilename), picture.PictureBinary);
                    }
                }
                else
                {
                    localFilename = string.Format("{0}_{1}.{2}", picture.PictureId.ToString("0000000"), targetSize, lastPart);
                    if (!File.Exists(Path.Combine(PictureManager.LocalThumbImagePath, localFilename)))
                    {
                        if (!System.IO.Directory.Exists(PictureManager.LocalThumbImagePath))
                        {
                            System.IO.Directory.CreateDirectory(PictureManager.LocalThumbImagePath);
                        }
                        using (MemoryStream stream = new MemoryStream(picture.PictureBinary))
                        {
                            var b = new Bitmap(stream);

                            var newSize = CalculateDimensions(b.Size, targetSize);

                            if (newSize.Width < 1)
                                newSize.Width = 1;
                            if (newSize.Height < 1)
                                newSize.Height = 1;

                            var newBitMap = new Bitmap(newSize.Width, newSize.Height);
                            var g = Graphics.FromImage(newBitMap);
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                            g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                            var ep = new EncoderParameters();
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureManager.ImageQuality);
                            newBitMap.Save(Path.Combine(PictureManager.LocalThumbImagePath, localFilename), getImageCodeInfo("image/jpeg"), ep);
                            newBitMap.Dispose();
                            b.Dispose();
                        }
                    }
                }
            }
            url = CommonHelper.GetStoreLocation() + "images/thumbs/" + localFilename;
            return url;
        }

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public static string GetPictureLocalPath(Picture picture, int targetSize, bool showDefaultPicture)
        {
            string url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if(String.IsNullOrEmpty(url))
            {
                return String.Empty;
            }
            else
            {
                return Path.Combine(PictureManager.LocalThumbImagePath, Path.GetFileName(url));
            }
        }

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static Size CalculateDimensions(Size originalSize, int targetSize)
        {
            var newSize = new Size();
            if (originalSize.Height > originalSize.Width) // portrait 
            {
                newSize.Width = (int)(originalSize.Width * (float)(targetSize / (float)originalSize.Height));
                newSize.Height = targetSize;
            }
            else // landscape or square
            {
                newSize.Height = (int)(originalSize.Height * (float)(targetSize / (float)originalSize.Width));
                newSize.Width = targetSize;
            }
            return newSize;
        }

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public static Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            var dbItem = DBProviderManager<DBPictureProvider>.Provider.GetPictureById(pictureId);
            if(!StoreInDB && dbItem != null)
            {
                dbItem.PictureBinary = LoadPictureFromFile(pictureId, dbItem.Extension);
            }
            var picture = DBMapping(dbItem);
            return picture;
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        public static void DeletePicture(int pictureId)
        {
            string filter = string.Format("{0}*.*", pictureId.ToString("0000000"));
            string[] currentFiles = System.IO.Directory.GetFiles(PictureManager.LocalThumbImagePath, filter);
            foreach (string currentFileName in currentFiles)
                File.Delete(Path.Combine(PictureManager.LocalThumbImagePath, currentFileName));

            DBProviderManager<DBPictureProvider>.Provider.DeletePicture(pictureId);
        }

        /// <summary>
        /// Validates input picture dimensions
        /// </summary>
        /// <param name="pictureBinary">Picture binary</param>
        /// <returns>Picture binary or throws an exception</returns>
        public static byte[] ValidatePicture(byte[] pictureBinary)
        {
            using (MemoryStream stream = new MemoryStream(pictureBinary))
            {
                var b = new Bitmap(stream);
                int maxSize = SettingManager.GetSettingValueInteger("Media.MaximumImageSize", 1280);

                if ((b.Height > maxSize) || (b.Width > maxSize))
                {
                    var newSize = CalculateDimensions(b.Size, maxSize);
                    var newBitMap = new Bitmap(newSize.Width, newSize.Height);
                    var g = Graphics.FromImage(newBitMap);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);

                    var m = new MemoryStream();
                    var ep = new EncoderParameters();
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureManager.ImageQuality);
                    newBitMap.Save(m, getImageCodeInfo("image/jpeg"), ep);
                    newBitMap.Dispose();
                    b.Dispose();

                    return m.GetBuffer();
                }
                else
                {
                    b.Dispose();
                    return pictureBinary;
                }
            }
        }
        
        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <param name="totalRecords">Output. how many records in results</param>
        /// <returns>Paged list of pictures</returns>
        public static PictureCollection GetPictures(int pageSize, 
            int pageIndex, out int totalRecords)
        {
            if (pageSize <= 0)
                pageSize = 10;
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            if (pageIndex < 0)
                pageIndex = 0;
            if (pageIndex == int.MaxValue)
                pageIndex = int.MaxValue - 1;

            var dbpics = DBProviderManager<DBPictureProvider>.Provider.GetPictures(pageSize, 
                pageIndex, out totalRecords);
            var pics = DBMapping(dbpics);
            return pics;
        }
        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="extension">The picture extension</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public static Picture InsertPicture(byte[] pictureBinary, string extension, bool isNew)
        {
            pictureBinary = ValidatePicture(pictureBinary);
            var dbItem = DBProviderManager<DBPictureProvider>.Provider.InsertPicture((StoreInDB ? pictureBinary : new byte[0]), extension, isNew);
            if(!StoreInDB && dbItem != null)
            {
                SavePictureInFile(dbItem.PictureId, pictureBinary, extension);
                dbItem.PictureBinary = pictureBinary;
            }
            var picture = DBMapping(dbItem);
            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="extension">The picture extension</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <returns>Picture</returns>
        public static Picture UpdatePicture(int pictureId, byte[] pictureBinary,
            string extension, bool isNew)
        {
            ValidatePicture(pictureBinary);
            var dbItem = DBProviderManager<DBPictureProvider>.Provider.UpdatePicture(pictureId,
                (StoreInDB ? pictureBinary : new byte[0]), extension, isNew);
            if(!StoreInDB && dbItem != null)
            {
                SavePictureInFile(dbItem.PictureId, pictureBinary, extension);
                dbItem.PictureBinary = pictureBinary;
            }
            var picture = DBMapping(dbItem);
            return picture;
        }

        /// <summary>
        /// Gets the picture binary array
        /// </summary>
        /// <param name="fs">File stream</param>
        /// <param name="size">Picture size</param>
        /// <returns>Picture binary array</returns>
        public static byte[] GetPictureBits(Stream fs, int size)
        {
            byte[] img = new byte[size];
            fs.Read(img, 0, size);
            return img;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets an image quality
        /// </summary>
        public static long ImageQuality
        {
            get
            {
                return 100L;
            }
        }

        /// <summary>
        /// Gets a local thumb image path
        /// </summary>
        public static string LocalThumbImagePath
        {
            get
            {
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "images\\thumbs";
                return path;
            }
        }

        /// <summary>
        /// Gets the local image path
        /// </summary>
        public static string LocalImagePath
        {
            get
            {
                string path = HttpContext.Current.Request.PhysicalApplicationPath + "images";
                return path;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the images should be stored in data base.
        /// </summary>
        public static bool StoreInDB
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Media.Images.StoreInDB", true);
            }
            set
            {
                SettingManager.SetParam("Media.Images.StoreInDB", value.ToString());
            }
        }
        #endregion
    }
}
