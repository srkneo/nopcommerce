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
using System.Text;
using System.Reflection;

namespace NopSolutions.NopCommerce.Common
{
    /// <summary>
    /// This class is used to clone object
    /// </summary>
    public sealed partial class NopCloneHelper<T> where T: new()
    {
        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public NopCloneHelper()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clone object
        /// </summary>
        /// <param name="obj">Object to clone</param>
        /// <returns>New object</returns>
        public static T CloneObject(T obj)
        {
            if (obj == null)
                return default(T);
            T newObject = Activator.CreateInstance<T>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                bool isDef = Attribute.IsDefined(propertyInfo,
                   typeof(NopClonableAttribute), true);
                if (isDef)
                {
                    //if (propertyInfo.PropertyType.IsGenericType)
                    //{
                    //    if (propertyInfo.PropertyType.GetInterface("IList", true) != null)
                    //    {
                    //        IList oldList = propertyInfo.GetValue(obj, null) as IList;
                    //        if (oldList != null && oldList.Count > 0 && oldList[0].GetType().GetInterface("IClonable", true) != null)
                    //        {
                    //            IList newList = (IList)propertyInfo.GetValue(newObject, null);
                    //            foreach (object obj2 in oldList)
                    //            {
                    //                ICloneable clone = (ICloneable)obj2;
                    //                newList.Add(clone.Clone());
                    //            }
                    //        }
                    //        else
                    //        {
                    //            propertyInfo.SetValue(newObject, oldList, null);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                        //Clone IClonable object
                        if (propertyInfo.GetType().GetInterface("IClonable", true) != null)
                        {
                            ICloneable clone = (ICloneable)propertyInfo.GetValue(obj, null);
                            propertyInfo.SetValue(newObject, clone.Clone(), null);
                        }
                        else
                        {
                            propertyInfo.SetValue(newObject, propertyInfo
                                .GetValue(obj, null), null);
                        }
                    //}
                }
            }
            return newObject;
        }
        #endregion
    }
}
