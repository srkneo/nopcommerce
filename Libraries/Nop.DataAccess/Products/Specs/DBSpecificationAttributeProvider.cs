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
using System.Data;
using System.Xml;

namespace NopSolutions.NopCommerce.DataAccess.Products.Specs
{
    /// <summary>
    /// Acts as a base class for deriving custom specification attribute provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/SpecificationAttributeProvider")]
    public abstract partial class DBSpecificationAttributeProvider : BaseDBProvider
    {
        #region Methods

        #region Specification attribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute</returns>
        public abstract DBSpecificationAttribute GetSpecificationAttributeById(int specificationAttributeId, int languageId);

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute collection</returns>
        public abstract DBSpecificationAttributeCollection GetSpecificationAttributes(int languageId);

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        public abstract void DeleteSpecificationAttribute(int specificationAttributeId);

        /// <summary>
        /// Inserts a specification attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public abstract DBSpecificationAttribute InsertSpecificationAttribute(string name, int displayOrder);

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public abstract DBSpecificationAttribute UpdateSpecificationAttribute(int specificationAttributeId, string name, int displayOrder);
        
        /// <summary>
        /// Gets localized specification attribute by id
        /// </summary>
        /// <param name="specificationAttributeLocalizedId">Localized specification identifier</param>
        /// <returns>Specification attribute content</returns>
        public abstract DBSpecificationAttributeLocalized GetSpecificationAttributeLocalizedById(int specificationAttributeLocalizedId);

        /// <summary>
        /// Gets localized specification attribute by specification attribute id and language id
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute content</returns>
        public abstract DBSpecificationAttributeLocalized GetSpecificationAttributeLocalizedBySpecificationAttributeIdAndLanguageId(int specificationAttributeId, int languageId);

        /// <summary>
        /// Inserts a localized specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public abstract DBSpecificationAttributeLocalized InsertSpecificationAttributeLocalized(int specificationAttributeId,
            int languageId, string name);

        /// <summary>
        /// Update a localized specification attribute
        /// </summary>
        /// <param name="specificationAttributeLocalizedId">Localized specification attribute identifier</param>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public abstract DBSpecificationAttributeLocalized UpdateSpecificationAttributeLocalized(int specificationAttributeLocalizedId,
            int specificationAttributeId, int languageId, string name);
      
        #endregion

        #region Specification attribute option

        /// <summary>
        /// Gets a specification attribute option collection
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public abstract DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptions(int languageId);

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option</returns>
        public abstract DBSpecificationAttributeOption GetSpecificationAttributeOptionById(int specificationAttributeOptionId, int languageId);

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute unique identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public abstract DBSpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttributeId(int specificationAttributeId, int languageId);

        /// <summary>
        /// Inserts a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public abstract DBSpecificationAttributeOption InsertSpecificationAttributeOption(int specificationAttributeId, 
            string name, int displayOrder);

        /// <summary>
        /// Updates the specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public abstract DBSpecificationAttributeOption UpdateSpecificationAttributeOption(int specificationAttributeOptionId, 
            int specificationAttributeId, string name, int displayOrder);

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        public abstract void DeleteSpecificationAttributeOption(int specificationAttributeOptionId);

        /// <summary>
        /// Gets localized specification attribute option by id
        /// </summary>
        /// <param name="specificationAttributeOptionLocalizedId">Localized specification attribute option identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public abstract DBSpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedById(int specificationAttributeOptionLocalizedId);

        /// <summary>
        /// Gets localized specification attribute option by specification attribute option id and language id
        /// </summary>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public abstract DBSpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIdAndLanguageId(int specificationAttributeOptionId, int languageId);

        /// <summary>
        /// Inserts a localized specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public abstract DBSpecificationAttributeOptionLocalized InsertSpecificationAttributeOptionLocalized(int specificationAttributeOptionId,
            int languageId, string name);

        /// <summary>
        /// Update a localized specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionLocalizedId">Localized specification attribute option identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public abstract DBSpecificationAttributeOptionLocalized UpdateSpecificationAttributeOptionLocalized(int specificationAttributeOptionLocalizedId,
            int specificationAttributeOptionId, int languageId, string name);
      
        #endregion
        
        #region Specification attribute option filter

        /// <summary>
        /// Gets all specification attribute option filter mapping collection by category id
        /// </summary>
        /// <param name="categoryId">Product category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option filter mapping collection</returns>
        public abstract DBSpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilterByCategoryId(int categoryId, int languageId);

        #endregion

        #endregion
    }
}
