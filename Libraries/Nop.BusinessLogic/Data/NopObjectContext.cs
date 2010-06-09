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
using System.Data.Objects;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Campaigns;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;


namespace NopSolutions.NopCommerce.BusinessLogic.Data
{
    /// <summary>
    /// Represents a nopCommerce object context
    /// </summary>
    public partial class NopObjectContext : ObjectContext
    {
        #region Fields

        private readonly Dictionary<Type, object> _entitySets;

        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the NopObjectContext class
        /// </summary>
        public NopObjectContext()
            : this("name=NopEntities")
        {

        }

        /// <summary>
        /// Creates a new instance of the NopObjectContext class
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public NopObjectContext(string connectionString)
            : base(connectionString, "NopEntities")
        {
            _entitySets = new Dictionary<Type, object>();
            this.ContextOptions.LazyLoadingEnabled = true;
        }
        #endregion

        #region Properties

        public ObjectSet<T> EntitySet<T>()
            where T : BaseEntity
        {
            var t = typeof(T);
            object match;

            if (!_entitySets.TryGetValue(t, out match))
            {
                match = CreateObjectSet<T>();
                _entitySets.Add(t, match);
            }

            return (ObjectSet<T>)match;
        }

        public ObjectSet<Affiliate> Affiliates
        {
            get
            {
                if ((_affiliates == null))
                {
                    _affiliates = CreateObjectSet<Affiliate>();
                }
                return _affiliates;
            }
        }
        private ObjectSet<Affiliate> _affiliates;

        public ObjectSet<BannedIpAddress> BannedIpAddresses
        {
            get
            {
                if ((_bannedIpAddresses == null))
                {
                    _bannedIpAddresses = CreateObjectSet<BannedIpAddress>();
                }
                return _bannedIpAddresses;
            }
        }
        private ObjectSet<BannedIpAddress> _bannedIpAddresses;

        public ObjectSet<BannedIpNetwork> BannedIpNetworks
        {
            get
            {
                if ((_bannedIpNetworks == null))
                {
                    _bannedIpNetworks = CreateObjectSet<BannedIpNetwork>();
                }
                return _bannedIpNetworks;
            }
        }
        private ObjectSet<BannedIpNetwork> _bannedIpNetworks;

        public ObjectSet<Campaign> Campaigns
        {
            get
            {
                if ((_campaigns == null))
                {
                    _campaigns = CreateObjectSet<Campaign>();
                }
                return _campaigns;
            }
        }
        private ObjectSet<Campaign> _campaigns;
        
        public ObjectSet<CategoryTemplate> CategoryTemplates
        {
            get
            {
                if ((_categoryTemplates == null))
                {
                    _categoryTemplates = CreateObjectSet<CategoryTemplate>();
                }
                return _categoryTemplates;
            }
        }
        private ObjectSet<CategoryTemplate> _categoryTemplates;

        public ObjectSet<Country> Countries
        {
            get
            {
                if ((_countries == null))
                {
                    _countries = CreateObjectSet<Country>();
                }
                return _countries;
            }
        }
        private ObjectSet<Country> _countries;

        public ObjectSet<CreditCardType> CreditCardTypes
        {
            get
            {
                if ((_creditCardTypes == null))
                {
                    _creditCardTypes = CreateObjectSet<CreditCardType>();
                }
                return _creditCardTypes;
            }
        }
        private ObjectSet<CreditCardType> _creditCardTypes;

        public ObjectSet<Currency> Currencies
        {
            get
            {
                if ((_currencies == null))
                {
                    _currencies = CreateObjectSet<Currency>();
                }
                return _currencies;
            }
        }
        private ObjectSet<Currency> _currencies;

        public ObjectSet<CustomerAction> CustomerActions
        {
            get
            {
                if ((_customerActions == null))
                {
                    _customerActions = CreateObjectSet<CustomerAction>();
                }
                return _customerActions;
            }
        }
        private ObjectSet<CustomerAction> _customerActions;

        public ObjectSet<DiscountLimitation> DiscountLimitations
        {
            get
            {
                if ((_discountLimitations == null))
                {
                    _discountLimitations = CreateObjectSet<DiscountLimitation>();
                }
                return _discountLimitations;
            }
        }
        private ObjectSet<DiscountLimitation> _discountLimitations;

        public ObjectSet<DiscountRequirement> DiscountRequirements
        {
            get
            {
                if ((_discountRequirements == null))
                {
                    _discountRequirements = CreateObjectSet<DiscountRequirement>();
                }
                return _discountRequirements;
            }
        }
        private ObjectSet<DiscountRequirement> _discountRequirements;

        public ObjectSet<DiscountType> DiscountTypes
        {
            get
            {
                if ((_discountTypes == null))
                {
                    _discountTypes = CreateObjectSet<DiscountType>();
                }
                return _discountTypes;
            }
        }
        private ObjectSet<DiscountType> _discountTypes;

        public ObjectSet<Download> Downloads
        {
            get
            {
                if ((_downloads == null))
                {
                    _downloads = CreateObjectSet<Download>();
                }
                return _downloads;
            }
        }
        private ObjectSet<Download> _downloads;

        public ObjectSet<Language> Languages
        {
            get
            {
                if ((_languages == null))
                {
                    _languages = CreateObjectSet<Language>();
                }
                return _languages;
            }
        }
        private ObjectSet<Language> _languages;

        public ObjectSet<LocalizedMessageTemplate> LocalizedMessageTemplates
        {
            get
            {
                if ((_localizedMessageTemplates == null))
                {
                    _localizedMessageTemplates = CreateObjectSet<LocalizedMessageTemplate>();
                }
                return _localizedMessageTemplates;
            }
        }
        private ObjectSet<LocalizedMessageTemplate> _localizedMessageTemplates;

        public ObjectSet<LocalizedTopic> LocalizedTopics
        {
            get
            {
                if ((_localizedTopics == null))
                {
                    _localizedTopics = CreateObjectSet<LocalizedTopic>();
                }
                return _localizedTopics;
            }
        }
        private ObjectSet<LocalizedTopic> _localizedTopics;

        public ObjectSet<ManufacturerTemplate> ManufacturerTemplates
        {
            get
            {
                if ((_manufacturerTemplates == null))
                {
                    _manufacturerTemplates = CreateObjectSet<ManufacturerTemplate>();
                }
                return _manufacturerTemplates;
            }
        }
        private ObjectSet<ManufacturerTemplate> _manufacturerTemplates;

        public ObjectSet<MeasureDimension> MeasureDimensions
        {
            get
            {
                if ((_measureDimensions == null))
                {
                    _measureDimensions = CreateObjectSet<MeasureDimension>();
                }
                return _measureDimensions;
            }
        }
        private ObjectSet<MeasureDimension> _measureDimensions;

        public ObjectSet<MeasureWeight> MeasureWeights
        {
            get
            {
                if ((_measureWeights == null))
                {
                    _measureWeights = CreateObjectSet<MeasureWeight>();
                }
                return _measureWeights;
            }
        }
        private ObjectSet<MeasureWeight> _measureWeights;

        public ObjectSet<MessageTemplate> MessageTemplates
        {
            get
            {
                if ((_messageTemplates == null))
                {
                    _messageTemplates = CreateObjectSet<MessageTemplate>();
                }
                return _messageTemplates;
            }
        }
        private ObjectSet<MessageTemplate> _messageTemplates;

        public ObjectSet<OrderStatus> OrderStatuses
        {
            get
            {
                if ((_orderStatuses == null))
                {
                    _orderStatuses = CreateObjectSet<OrderStatus>();
                }
                return _orderStatuses;
            }
        }
        private ObjectSet<OrderStatus> _orderStatuses;

        public ObjectSet<PaymentMethod> PaymentMethods
        {
            get
            {
                if ((_paymentMethods == null))
                {
                    _paymentMethods = CreateObjectSet<PaymentMethod>();
                }
                return _paymentMethods;
            }
        }
        private ObjectSet<PaymentMethod> _paymentMethods;

        public ObjectSet<PaymentStatus> PaymentStatuses
        {
            get
            {
                if ((_paymentStatuses == null))
                {
                    _paymentStatuses = CreateObjectSet<PaymentStatus>();
                }
                return _paymentStatuses;
            }
        }
        private ObjectSet<PaymentStatus> _paymentStatuses;

        public ObjectSet<ProductTemplate> ProductTemplates
        {
            get
            {
                if ((_productTemplates == null))
                {
                    _productTemplates = CreateObjectSet<ProductTemplate>();
                }
                return _productTemplates;
            }
        }
        private ObjectSet<ProductTemplate> _productTemplates;

        public ObjectSet<QueuedEmail> QueuedEmails
        {
            get
            {
                if ((_queuedEmails == null))
                {
                    _queuedEmails = CreateObjectSet<QueuedEmail>();
                }
                return _queuedEmails;
            }
        }
        private ObjectSet<QueuedEmail> _queuedEmails;

        public ObjectSet<ShippingRateComputationMethod> ShippingRateComputationMethods
        {
            get
            {
                if ((_shippingRateComputationMethods == null))
                {
                    _shippingRateComputationMethods = CreateObjectSet<ShippingRateComputationMethod>();
                }
                return _shippingRateComputationMethods;
            }
        }
        private ObjectSet<ShippingRateComputationMethod> _shippingRateComputationMethods;

        public ObjectSet<ShippingStatus> ShippingStatuses
        {
            get
            {
                if ((_shippingStatuses == null))
                {
                    _shippingStatuses = CreateObjectSet<ShippingStatus>();
                }
                return _shippingStatuses;
            }
        }
        private ObjectSet<ShippingStatus> _shippingStatuses;

        public ObjectSet<StateProvince> StateProvinces
        {
            get
            {
                if ((_stateProvinces == null))
                {
                    _stateProvinces = CreateObjectSet<StateProvince>();
                }
                return _stateProvinces;
            }
        }
        private ObjectSet<StateProvince> _stateProvinces;

        public ObjectSet<Topic> Topics
        {
            get
            {
                if ((_topics == null))
                {
                    _topics = CreateObjectSet<Topic>();
                }
                return _topics;
            }
        }
        private ObjectSet<Topic> _topics;

        public ObjectSet<TaxCategory> TaxCategories
        {
            get
            {
                if ((_taxCategories == null))
                {
                    _taxCategories = CreateObjectSet<TaxCategory>();
                }
                return _taxCategories;
            }
        }
        private ObjectSet<TaxCategory> _taxCategories;

        public ObjectSet<TaxProvider> TaxProviders
        {
            get
            {
                if ((_taxProviders == null))
                {
                    _taxProviders = CreateObjectSet<TaxProvider>();
                }
                return _taxProviders;
            }
        }
        private ObjectSet<TaxProvider> _taxProviders;

        public ObjectSet<Warehouse> Warehouses
        {
            get
            {
                if ((_warehouses == null))
                {
                    _warehouses = CreateObjectSet<Warehouse>();
                }
                return _warehouses;
            }
        }
        private ObjectSet<Warehouse> _warehouses;

        #endregion
    }

}
