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
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.BusinessLogic.Templates;


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
