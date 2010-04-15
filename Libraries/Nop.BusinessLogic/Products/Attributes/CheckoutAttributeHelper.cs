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
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.Products.Attributes
{
    /// <summary>
    /// Checkout attribute helper
    /// </summary>
    public class CheckoutAttributeHelper
    {
        #region Product attributes

        /// <summary>
        /// Gets selected checkout attribute identifiers
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Selected checkout attribute identifiers</returns>
        public static List<int> ParseCheckoutAttributeIDs(string Attributes)
        {
            var IDs = new List<int>();
            if (String.IsNullOrEmpty(Attributes))
                return IDs;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CheckoutAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        IDs.Add(id);
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return IDs;
        }

        /// <summary>
        /// Gets selected checkout attributes
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Selected checkout attributes</returns>
        public static CheckoutAttributeCollection ParseCheckoutAttributes(string Attributes)
        {
            var caCollection = new CheckoutAttributeCollection();
            var IDs = ParseCheckoutAttributeIDs(Attributes);
            foreach (int id in IDs)
            {
                var ca = CheckoutAttributeManager.GetCheckoutAttributeByID(id);
                if (ca != null)
                {
                    caCollection.Add(ca);
                }
            }
            return caCollection;
        }

        /// <summary>
        /// Get checkout attribute values
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Checkout attribute values</returns>
        public static CheckoutAttributeValueCollection ParseCheckoutAttributeValues(string Attributes)
        {
            var caValues = new CheckoutAttributeValueCollection();
            var caCollection = ParseCheckoutAttributes(Attributes);
            foreach (var ca in caCollection)
            {
                if (!ca.ShouldHaveValues)
                    continue;

                var caValuesStr = ParseValues(Attributes, ca.CheckoutAttributeID);
                foreach (string caValueStr in caValuesStr)
                {
                    if (!String.IsNullOrEmpty(caValueStr))
                    {
                        int caValueID = 0;
                        if (int.TryParse(caValueStr, out caValueID))
                        {
                            var caValue = CheckoutAttributeManager.GetCheckoutAttributeValueByID(caValueID);
                            if (caValue != null)
                                caValues.Add(caValue);
                        }
                    }
                }
            }
            return caValues;
        }

        /// <summary>
        /// Gets selected checkout attribute value
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <returns>Checkout attribute value</returns>
        public static List<string> ParseValues(string Attributes, int CheckoutAttributeID)
        {
            var selectedCheckoutAttributeValues = new List<string>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CheckoutAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        if (id == CheckoutAttributeID)
                        {
                            XmlNodeList nodeList2 = node1.SelectNodes(@"CheckoutAttributeValue/Value");
                            foreach (XmlNode node2 in nodeList2)
                            {
                                string value = node2.InnerText.Trim();
                                selectedCheckoutAttributeValues.Add(value);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return selectedCheckoutAttributeValues;
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="ca">Checkout attribute</param>
        /// <param name="value">Value</param>
        /// <returns>Attributes</returns>
        public static string AddCheckoutAttribute(string Attributes, CheckoutAttribute ca, string value)
        {
            string result = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (String.IsNullOrEmpty(Attributes))
                {
                    XmlElement _element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(_element1);
                }
                else
                {
                    xmlDoc.LoadXml(Attributes);
                }
                XmlElement rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement caElement = null;
                //find existing
                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/CheckoutAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        if (id == ca.CheckoutAttributeID)
                        {
                            caElement = (XmlElement)node1;
                            break;
                        }
                    }
                }

                //create new one if not found
                if (caElement == null)
                {
                    caElement = xmlDoc.CreateElement("CheckoutAttribute");
                    caElement.SetAttribute("ID", ca.CheckoutAttributeID.ToString());
                    rootElement.AppendChild(caElement);
                }

                XmlElement cavElement = xmlDoc.CreateElement("CheckoutAttributeValue");
                caElement.AppendChild(cavElement);

                XmlElement cavVElement = xmlDoc.CreateElement("Value");
                cavVElement.InnerText = value;
                cavElement.AppendChild(cavVElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return result;
        }

        #endregion

        #region Formatting

        #endregion
    }
}
