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
using Nop.Data;
using Nop.Core.Domain.Orders;
using Nop.Core.Caching;

namespace Nop.Services.Orders
{
    /// <summary>
    /// Checkout attribute service
    /// </summary>
    public partial class CheckoutAttributeService : ICheckoutAttributeService
    {
        #region Constants
        private const string CHECKOUTATTRIBUTES_ALL_KEY = "Nop.checkoutattribute.all-{0}";
        private const string CHECKOUTATTRIBUTES_BY_ID_KEY = "Nop.checkoutattribute.id-{0}";
        private const string CHECKOUTATTRIBUTEVALUES_ALL_KEY = "Nop.checkoutattributevalue.all-{0}";
        private const string CHECKOUTATTRIBUTEVALUES_BY_ID_KEY = "Nop.checkoutattributevalue.id-{0}";
        private const string CHECKOUTATTRIBUTES_PATTERN_KEY = "Nop.checkoutattribute.";
        private const string CHECKOUTATTRIBUTEVALUES_PATTERN_KEY = "Nop.checkoutattributevalue.";
        #endregion
        
        #region Fields

        private readonly IRepository<CheckoutAttribute> _checkoutAttributeRepository;
        private readonly IRepository<CheckoutAttributeValue> _checkoutAttributeValueRepository;
        private readonly ICacheManager _cacheManager;
        
        #endregion

        #region Ctor
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="checkoutAttributeRepository">Checkout attribute repository</param>
        /// <param name="checkoutAttributeValueRepository">Checkout attribute value repository</param>
        public CheckoutAttributeService(ICacheManager cacheManager,
            IRepository<CheckoutAttribute> checkoutAttributeRepository,
            IRepository<CheckoutAttributeValue> checkoutAttributeValueRepository)
        {
            this._cacheManager = cacheManager;
            this._checkoutAttributeRepository = checkoutAttributeRepository;
            this._checkoutAttributeValueRepository = checkoutAttributeValueRepository;
        }

        #endregion

        #region Methods

        #region Checkout attributes

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public void DeleteCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                return;

            _checkoutAttributeRepository.Delete(checkoutAttribute);

            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="dontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public IList<CheckoutAttribute> GetAllCheckoutAttributes(bool dontLoadShippableProductRequired)
        {
            string key = string.Format(CHECKOUTATTRIBUTES_ALL_KEY, dontLoadShippableProductRequired);
            return _cacheManager.Get(key, () =>
            {
                var query = from ca in _checkoutAttributeRepository.Table
                            orderby ca.DisplayOrder
                            where !dontLoadShippableProductRequired || !ca.ShippableProductRequired
                            select ca;
                var checkoutAttributes = query.ToList();
                return checkoutAttributes;
            });
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <returns>Checkout attribute</returns>
        public CheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId)
        {
            if (checkoutAttributeId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTES_BY_ID_KEY, checkoutAttributeId);
            return _cacheManager.Get(key, () =>
            {
                var checkoutAttribute = _checkoutAttributeRepository.GetById(checkoutAttributeId);
                return checkoutAttribute;
            });
        }

        /// <summary>
        /// Inserts a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public void InsertCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Insert(checkoutAttribute);

            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public void UpdateCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException("checkoutAttribute");

            _checkoutAttributeRepository.Update(checkoutAttribute);

            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
        }

        #endregion

        #region Checkout variant attribute values

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public void DeleteCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                return;

            _checkoutAttributeValueRepository.Delete(checkoutAttributeValue);

            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public IList<CheckoutAttributeValue> GetCheckoutAttributeValues(int checkoutAttributeId)
        {
            string key = string.Format(CHECKOUTATTRIBUTEVALUES_ALL_KEY, checkoutAttributeId);
            return _cacheManager.Get(key, () =>
            {
                var query = from cav in _checkoutAttributeValueRepository.Table
                            orderby cav.DisplayOrder
                            where cav.CheckoutAttributeId == checkoutAttributeId
                            select cav;
                var checkoutAttributeValues = query.ToList();
                return checkoutAttributeValues;
            });
        }
        
        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <returns>Checkout attribute value</returns>
        public CheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId)
        {
            if (checkoutAttributeValueId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTEVALUES_BY_ID_KEY, checkoutAttributeValueId);
            return _cacheManager.Get(key, () =>
            {
                var checkoutAttributeValue = _checkoutAttributeValueRepository.GetById(checkoutAttributeValueId);
                return checkoutAttributeValue;
            });
        }

        /// <summary>
        /// Inserts a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public void InsertCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Insert(checkoutAttributeValue);

            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public void UpdateCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException("checkoutAttributeValue");

            _checkoutAttributeValueRepository.Update(checkoutAttributeValue);

            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
        }
        
        #endregion

        #endregion
    }
}