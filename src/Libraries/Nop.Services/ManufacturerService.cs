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
using System.Linq;
using Nop.Core.Domain;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Core;

namespace Nop.Services
{
    /// <summary>
    /// Manufacturer service
    /// </summary>
    public partial class ManufacturerService : IManufacturerService
    {
        #region Constants
        private const string MANUFACTURERS_ALL_KEY = "Nop.manufacturer.all-{0}";
        private const string MANUFACTURERS_BY_ID_KEY = "Nop.manufacturer.id-{0}";
        private const string PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY = "Nop.productmanufacturer.allbymanufacturerid-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY = "Nop.productmanufacturer.allbyproductid-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_BY_ID_KEY = "Nop.productmanufacturer.id-{0}";
        private const string MANUFACTURERS_PATTERN_KEY = "Nop.manufacturer.";
        private const string PRODUCTMANUFACTURERS_PATTERN_KEY = "Nop.productmanufacturer.";
        #endregion

        #region Fields

        private readonly IRepository<Manufacturer> _manufacturerRespository;
        private readonly IRepository<LocalizedManufacturer> _localizedManufacturerRespository;
        private readonly IRepository<ProductManufacturer> _productManufacturerRespository;
        private readonly IRepository<Product> _productRespository;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="categoryRespository">Category repository</param>
        /// <param name="localizedCategoryRespository">Localized category repository</param>
        /// <param name="productCategoryRespository">ProductCategory repository</param>
        /// <param name="productRespository">Product repository</param>
        public ManufacturerService(ICacheManager cacheManager,
            IRepository<Manufacturer> manufacturerRespository,
            IRepository<LocalizedManufacturer> localizedManufacturerRespository,
            IRepository<ProductManufacturer> productManufacturerRespository,
            IRepository<Product> productRespository)
        {
            this._cacheManager = cacheManager;
            this._manufacturerRespository = manufacturerRespository;
            this._localizedManufacturerRespository = localizedManufacturerRespository;
            this._productManufacturerRespository = productManufacturerRespository;
            this._productRespository = productRespository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Deletes a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public void DeleteManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                return;

            manufacturer.Deleted = true;
            UpdateManufacturer(manufacturer);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <returns>Manufacturer collection</returns>
        public List<Manufacturer> GetAllManufacturers()
        {
            //TODO: use bool showHidden = NopContext.Current.IsAdmin;
            bool showHidden = true;
            return GetAllManufacturers(showHidden);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturer collection</returns>
        public List<Manufacturer> GetAllManufacturers(bool showHidden)
        {
            string key = string.Format(MANUFACTURERS_ALL_KEY, showHidden);
            object obj2 = _cacheManager.Get(key);
            if (obj2 != null)
            {
                return (List<Manufacturer>)obj2;
            }

            
            var query = from m in _manufacturerRespository.Table
                        orderby m.DisplayOrder
                        where (showHidden || m.Published) &&
                        !m.Deleted
                        select m;
            var manufacturers = query.ToList();

            //cache
                _cacheManager.Add(key, manufacturers);
            
            return manufacturers;
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public Manufacturer GetManufacturerById(int manufacturerId)
        {
            if (manufacturerId == 0)
                return null;

            string key = string.Format(MANUFACTURERS_BY_ID_KEY, manufacturerId);
            object obj2 = _cacheManager.Get(key);
            if (obj2 != null)
            {
                return (Manufacturer)obj2;
            }
            
            var manufacturer = _manufacturerRespository.GetById(manufacturerId);

            //cache
                _cacheManager.Add(key, manufacturer);

            return manufacturer;
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public void InsertManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            _manufacturerRespository.Insert(manufacturer);

            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="manufacturer">Manufacturer</param>
        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new ArgumentNullException("manufacturer");

            _manufacturerRespository.Update(manufacturer);

            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets localized manufacturer by id
        /// </summary>
        /// <param name="localizedManufacturerId">Localized manufacturer identifier</param>
        /// <returns>Manufacturer content</returns>
        public LocalizedManufacturer GetLocalizedManufacturerById(int localizedManufacturerId)
        {
            if (localizedManufacturerId == 0)
                return null;

            var manufacturerLocalized = _localizedManufacturerRespository.GetById(localizedManufacturerId);
            
            return manufacturerLocalized;
        }

        /// <summary>
        /// Gets localized manufacturer by manufacturer id
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Manufacturer content</returns>
        public List<LocalizedManufacturer> GetLocalizedManufacturerByManufacturerId(int manufacturerId)
        {
            if (manufacturerId == 0)
                return new List<LocalizedManufacturer>();
            
            var query = from ml in _localizedManufacturerRespository.Table
                        where ml.ManufacturerId == manufacturerId
                        select ml;
            var content = query.ToList();
            return content;
        }

        /// <summary>
        /// Gets localized manufacturer by manufacturer id and language id
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer content</returns>
        public LocalizedManufacturer GetLocalizedManufacturerByManufacturerIdAndLanguageId(int manufacturerId, int languageId)
        {
            if (manufacturerId == 0 || languageId == 0)
                return null;

            var query = from ml in _localizedManufacturerRespository.Table
                        orderby ml.Id
                        where ml.ManufacturerId == manufacturerId &&
                        ml.LanguageId == languageId
                        select ml;
            var manufacturerLocalized = query.FirstOrDefault();
            return manufacturerLocalized;
        }

        /// <summary>
        /// Inserts a localized manufacturer
        /// </summary>
        /// <param name="localizedManufacturer">Localized manufacturer</param>
        public void InsertLocalizedManufacturer(LocalizedManufacturer localizedManufacturer)
        {
            if (localizedManufacturer == null)
                throw new ArgumentNullException("localizedManufacturer");

            _localizedManufacturerRespository.Insert(localizedManufacturer);
         
            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
        }

        /// <summary>
        /// Update a localized manufacturer
        /// </summary>
        /// <param name="localizedManufacturer">Localized manufacturer</param>
        public void UpdateLocalizedManufacturer(LocalizedManufacturer localizedManufacturer)
        {
            if (localizedManufacturer == null)
                throw new ArgumentNullException("localizedManufacturer");

            bool allFieldsAreEmpty = string.IsNullOrEmpty(localizedManufacturer.Name) &&
                                     string.IsNullOrEmpty(localizedManufacturer.Description) &&
                                     string.IsNullOrEmpty(localizedManufacturer.MetaKeywords) &&
                                     string.IsNullOrEmpty(localizedManufacturer.MetaDescription) &&
                                     string.IsNullOrEmpty(localizedManufacturer.MetaTitle) &&
                                     string.IsNullOrEmpty(localizedManufacturer.SeName);

            if (allFieldsAreEmpty)
            {
                //delete if all fields are empty
                _localizedManufacturerRespository.Delete(localizedManufacturer);
            }
            else
            {
                _localizedManufacturerRespository.Update(localizedManufacturer);
            }

            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
        }

        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturer">Product manufacturer mapping</param>
        public void DeleteProductManufacturer(ProductManufacturer productManufacturer)
        {
            if (productManufacturer == null)
                return;

            _productManufacturerRespository.Delete(productManufacturer);

            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets product manufacturer collection
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Product manufacturer collection</returns>
        public List<ProductManufacturer> GetProductManufacturersByManufacturerId(int manufacturerId)
        {
            if (manufacturerId == 0)
                return new List<ProductManufacturer>();

            //TODO: use bool showHidden = NopContext.Current.IsAdmin;
            bool showHidden = true;

            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY, showHidden, manufacturerId);
            object obj2 = _cacheManager.Get(key);
            if (obj2 != null)
            {
                return (List<ProductManufacturer>) obj2;
            }
            
            var query = from pm in _productManufacturerRespository.Table
                        join p in _productRespository.Table on pm.ProductId equals p.Id
                        where pm.ManufacturerId == manufacturerId &&
                              !p.Deleted &&
                              (showHidden || p.Published)
                        orderby pm.DisplayOrder
                        select pm;
            var productManufacturers = query.ToList();

            //cache
            _cacheManager.Add(key, productManufacturers);

            return productManufacturers;
        }

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public List<ProductManufacturer> GetProductManufacturersByProductId(int productId)
        {
            if (productId == 0)
                return new List<ProductManufacturer>();

            //TODO: use bool showHidden = NopContext.Current.IsAdmin;
            bool showHidden = true;

            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY, showHidden, productId);
            object obj2 = _cacheManager.Get(key);
            if (obj2 != null)
            {
                return (List<ProductManufacturer>) obj2;
            }


            var query = from pm in _productManufacturerRespository.Table
                        join m in _manufacturerRespository.Table on pm.ManufacturerId equals m.Id
                        where pm.ProductId == productId &&
                              !m.Deleted &&
                              (showHidden || m.Published)
                        orderby pm.DisplayOrder
                        select pm;
            var productManufacturers = query.ToList();

            //cache
            _cacheManager.Add(key, productManufacturers);

            return productManufacturers;
        }

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public ProductManufacturer GetProductManufacturerById(int productManufacturerId)
        {
            if (productManufacturerId == 0)
                return null;

            string key = string.Format(PRODUCTMANUFACTURERS_BY_ID_KEY, productManufacturerId);
            object obj2 = _cacheManager.Get(key);
            if (obj2 != null)
            {
                return (ProductManufacturer)obj2;
            }

            var productManufacturer = _productManufacturerRespository.GetById(productManufacturerId);

            //cache
                _cacheManager.Add(key, productManufacturer);
            
            return productManufacturer;
        }

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturer">Product manufacturer mapping</param>
        public void InsertProductManufacturer(ProductManufacturer productManufacturer)
        {
            if (productManufacturer == null)
                throw new ArgumentNullException("productManufacturer");

            _productManufacturerRespository.Insert(productManufacturer);

            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturer">Product manufacturer mapping</param>
        public void UpdateProductManufacturer(ProductManufacturer productManufacturer)
        {
            if (productManufacturer == null)
                throw new ArgumentNullException("productManufacturer");

            _productManufacturerRespository.Update(productManufacturer);

            //cache
            _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            _cacheManager.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
        }

        #endregion
    }
}
