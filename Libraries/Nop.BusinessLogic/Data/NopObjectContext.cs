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


namespace NopSolutions.NopCommerce.BusinessLogic.Data
{
    /// <summary>
    /// Represents a nopCommerce object context
    /// </summary>
    public partial class NopObjectContext : ObjectContext
    {
        private readonly Dictionary<Type, object> _entitySets;

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

        #endregion
    }

}
