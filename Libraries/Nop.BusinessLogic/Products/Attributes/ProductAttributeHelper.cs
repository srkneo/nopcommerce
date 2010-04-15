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
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.Products.Attributes
{
    /// <summary>
    /// Product attribute helper
    /// </summary>
    public class ProductAttributeHelper
    {
        #region Product attributes

        /// <summary>
        /// Gets selected product variant attribute identifiers
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Selected product variant attribute identifiers</returns>
        public static List<int> ParseProductVariantAttributeIDs(string Attributes)
        {
            var IDs = new List<int>();
            if (String.IsNullOrEmpty(Attributes))
                return IDs;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
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
        /// Gets selected product variant attributes
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Selected product variant attributes</returns>
        public static ProductVariantAttributeCollection ParseProductVariantAttributes(string Attributes)
        {
            var pvaCollection = new ProductVariantAttributeCollection();
            var IDs = ParseProductVariantAttributeIDs(Attributes);
            foreach (int id in IDs)
            {
                var pva = ProductAttributeManager.GetProductVariantAttributeByID(id);
                if (pva != null)
                {
                    pvaCollection.Add(pva);
                }
            }
            return pvaCollection;
        }

        /// <summary>
        /// Get product variant attribute values
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Product variant attribute values</returns>
        public static ProductVariantAttributeValueCollection ParseProductVariantAttributeValues(string Attributes)
        {
            var pvaValues = new ProductVariantAttributeValueCollection();
            var pvaCollection = ParseProductVariantAttributes(Attributes);
            foreach (var pva in pvaCollection)
            {
                if (!pva.ShouldHaveValues)
                    continue;

                var pvaValuesStr = ParseValues(Attributes, pva.ProductVariantAttributeID);
                foreach (string pvaValueStr in pvaValuesStr)
                {
                    if (!String.IsNullOrEmpty(pvaValueStr))
                    {
                        int pvaValueID = 0;
                        if (int.TryParse(pvaValueStr, out pvaValueID))
                        {
                            var pvaValue = ProductAttributeManager.GetProductVariantAttributeValueByID(pvaValueID);
                            if (pvaValue != null)
                                pvaValues.Add(pvaValue);
                        }
                    }
                }
            }
            return pvaValues;
        }

        /// <summary>
        /// Gets selected product variant attribute value
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="ProductVariantAttributeID">Product variant attribute identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static List<string> ParseValues(string Attributes, int ProductVariantAttributeID)
        {
            var selectedProductVariantAttributeValues = new List<string>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        if (id == ProductVariantAttributeID)
                        {
                            XmlNodeList nodeList2 = node1.SelectNodes(@"ProductVariantAttributeValue/Value");
                            foreach (XmlNode node2 in nodeList2)
                            {
                                string value = node2.InnerText.Trim();
                                selectedProductVariantAttributeValues.Add(value);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return selectedProductVariantAttributeValues;
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="pva">Product variant attribute</param>
        /// <param name="value">Value</param>
        /// <returns>Attributes</returns>
        public static string AddProductAttribute(string Attributes, ProductVariantAttribute pva, string value)
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

                XmlElement pvaElement = null;
                //find existing
                XmlNodeList nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        int id = Convert.ToInt32(node1.Attributes["ID"].InnerText.Trim());
                        if (id == pva.ProductVariantAttributeID)
                        {
                            pvaElement = (XmlElement)node1;
                            break;
                        }
                    }
                }

                //create new one if not found
                if (pvaElement == null)
                {
                    pvaElement = xmlDoc.CreateElement("ProductVariantAttribute");
                    pvaElement.SetAttribute("ID", pva.ProductVariantAttributeID.ToString());
                    rootElement.AppendChild(pvaElement);
                }

                XmlElement pvavElement = xmlDoc.CreateElement("ProductVariantAttributeValue");
                pvaElement.AppendChild(pvavElement);

                XmlElement pvavVElement = xmlDoc.CreateElement("Value");
                pvavVElement.InnerText = value;
                pvavElement.AppendChild(pvavVElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return result;
        }

        /// <summary>
        /// Are attributes equal
        /// </summary>
        /// <param name="Attributes1">The attributes of the first product variant</param>
        /// <param name="Attributes2">The attributes of the second product variant</param>
        /// <returns>Result</returns>
        public static bool AreProductAttributesEqual(string Attributes1, string Attributes2)
        {
            bool attributesEqual = true;
            if (ProductAttributeHelper.ParseProductVariantAttributeIDs(Attributes1).Count == ProductAttributeHelper.ParseProductVariantAttributeIDs(Attributes2).Count)
            {
                var pva1Collection = ProductAttributeHelper.ParseProductVariantAttributes(Attributes2);
                var pva2Collection = ProductAttributeHelper.ParseProductVariantAttributes(Attributes1);
                foreach (var pva1 in pva1Collection)
                {
                    foreach (var pva2 in pva2Collection)
                    {
                        if (pva1.ProductVariantAttributeID == pva2.ProductVariantAttributeID)
                        {
                            var pvaValues1Str = ProductAttributeHelper.ParseValues(Attributes2, pva1.ProductVariantAttributeID);
                            var pvaValues2Str = ProductAttributeHelper.ParseValues(Attributes1, pva2.ProductVariantAttributeID);
                            if (pvaValues1Str.Count == pvaValues2Str.Count)
                            {
                                foreach (string str1 in pvaValues1Str)
                                {
                                    bool hasAttribute = false;
                                    foreach (string str2 in pvaValues2Str)
                                    {
                                        if (str1.Trim().ToLower() == str2.Trim().ToLower())
                                        {
                                            hasAttribute = true;
                                            break;
                                        }
                                    }

                                    if (!hasAttribute)
                                    {
                                        attributesEqual = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                attributesEqual = false;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                attributesEqual = false;
            }

            return attributesEqual;
        }

        #endregion

        #region Gift card attributes

        /// <summary>
        /// Add gift card attrbibutes
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="recipientName">Recipient name</param>
        /// <param name="recipientEmail">Recipient email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="giftCardMessage">Message</param>
        /// <returns>Attributes</returns>
        public static string AddGiftCardAttribute(string Attributes, string recipientName,
            string recipientEmail, string senderName, string senderEmail, string giftCardMessage)
        {
            string result = string.Empty;
            try
            {
                recipientName = recipientName.Trim();
                recipientEmail = recipientEmail.Trim();
                senderName = senderName.Trim();
                senderEmail = senderEmail.Trim();

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

                XmlElement giftCardElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes/GiftCardInfo");
                if (giftCardElement == null)
                {
                    giftCardElement = xmlDoc.CreateElement("GiftCardInfo");
                    rootElement.AppendChild(giftCardElement);
                }

                XmlElement recipientNameElement = xmlDoc.CreateElement("RecipientName");
                recipientNameElement.InnerText = recipientName;
                giftCardElement.AppendChild(recipientNameElement);

                XmlElement recipientEmailElement = xmlDoc.CreateElement("RecipientEmail");
                recipientEmailElement.InnerText = recipientEmail;
                giftCardElement.AppendChild(recipientEmailElement);

                XmlElement senderNameElement = xmlDoc.CreateElement("SenderName");
                senderNameElement.InnerText = senderName;
                giftCardElement.AppendChild(senderNameElement);

                XmlElement senderEmailElement = xmlDoc.CreateElement("SenderEmail");
                senderEmailElement.InnerText = senderEmail;
                giftCardElement.AppendChild(senderEmailElement);

                XmlElement messageElement = xmlDoc.CreateElement("Message");
                messageElement.InnerText = giftCardMessage;
                giftCardElement.AppendChild(messageElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return result;
        }

        /// <summary>
        /// Get gift card attrbibutes
        /// </summary>
        /// <param name="Attributes">Attributes</param>
        /// <param name="recipientName">Recipient name</param>
        /// <param name="recipientEmail">Recipient email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="giftCardMessage">Message</param>
        public static void GetGiftCardAttribute(string Attributes, out string recipientName,
            out string recipientEmail, out string senderName,
            out string senderEmail, out string giftCardMessage)
        {
            recipientName = string.Empty;
            recipientEmail = string.Empty;
            senderName = string.Empty;
            senderEmail = string.Empty;
            giftCardMessage = string.Empty;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(Attributes);

                XmlElement recipientNameElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes/GiftCardInfo/RecipientName");
                XmlElement recipientEmailElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes/GiftCardInfo/RecipientEmail");
                XmlElement senderNameElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes/GiftCardInfo/SenderName");
                XmlElement senderEmailElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes/GiftCardInfo/SenderEmail");
                XmlElement messageElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes/GiftCardInfo/Message");

                if (recipientNameElement != null)
                    recipientName = recipientNameElement.InnerText;
                if (recipientEmailElement != null)
                    recipientEmail = recipientEmailElement.InnerText;
                if (senderNameElement != null)
                    senderName = senderNameElement.InnerText;
                if (senderEmailElement != null)
                    senderEmail = senderEmailElement.InnerText;
                if (messageElement != null)
                    giftCardMessage = messageElement.InnerText;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
        }

        #endregion

        #region Formatting

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes)
        {
            var customer = NopContext.Current.User;
            return FormatAttributes(productVariant, Attributes, customer, "<br />");
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="Serapator">Serapator</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes, Customer customer, string Serapator)
        {
            return FormatAttributes(productVariant, Attributes, customer, Serapator, true);
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="Serapator">Serapator</param>
        /// <param name="HTMLEncode">A value indicating whether to encode (HTML) values</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes,
            Customer customer, string Serapator, bool HTMLEncode)
        {
            return FormatAttributes(productVariant, Attributes, customer, Serapator, HTMLEncode, true);
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="Serapator">Serapator</param>
        /// <param name="HTMLEncode">A value indicating whether to encode (HTML) values</param>
        /// <param name="RenderPrices">A value indicating whether to render prices</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes,
            Customer customer, string Serapator, bool HTMLEncode, bool RenderPrices)
        {
            return FormatAttributes(productVariant, Attributes, customer, Serapator, HTMLEncode, RenderPrices, true, true);
        }

        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Attributes">Attributes</param>
        /// <param name="customer">Customer</param>
        /// <param name="Serapator">Serapator</param>
        /// <param name="HTMLEncode">A value indicating whether to encode (HTML) values</param>
        /// <param name="RenderPrices">A value indicating whether to render prices</param>
        /// <param name="RenderProductAttributes">A value indicating whether to render product attributes</param>
        /// <param name="RenderGiftCardAttributes">A value indicating whether to render gift card attributes</param>
        /// <returns>Attributes</returns>
        public static string FormatAttributes(ProductVariant productVariant, string Attributes,
            Customer customer, string Serapator, bool HTMLEncode, bool RenderPrices,
            bool RenderProductAttributes, bool RenderGiftCardAttributes)
        {
            var result = new StringBuilder();

            //attributes
            if (RenderProductAttributes)
            {
                var pvaCollection = ParseProductVariantAttributes(Attributes);
                for (int i = 0; i < pvaCollection.Count; i++)
                {
                    var pva = pvaCollection[i];
                    var valuesStr = ParseValues(Attributes, pva.ProductVariantAttributeID);
                    for (int j = 0; j < valuesStr.Count; j++)
                    {
                        string valueStr = valuesStr[j];
                        string pvaAttribute = string.Empty;
                        if (!pva.ShouldHaveValues)
                        {
                            pvaAttribute = string.Format("{0}: {1}", pva.ProductAttribute.Name, valueStr);
                        }
                        else
                        {
                            var pvaValue = ProductAttributeManager.GetProductVariantAttributeValueByID(Convert.ToInt32(valueStr));
                            if (pvaValue != null)
                            {
                                pvaAttribute = string.Format("{0}: {1}", pva.ProductAttribute.Name, pvaValue.Name);
                                if (RenderPrices)
                                {
                                    decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment, customer);
                                    decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                    if (priceAdjustmentBase > 0)
                                    {
                                        string priceAdjustmentStr = PriceHelper.FormatPrice(priceAdjustment, false, false);
                                        pvaAttribute += string.Format(" [+{0}]", priceAdjustmentStr);
                                    }
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(pvaAttribute))
                        {
                            if (i != 0 || j != 0)
                            {
                                result.Append(Serapator);
                            }

                            if (HTMLEncode)
                            {
                                result.Append(HttpUtility.HtmlEncode(pvaAttribute));
                            }
                            else
                            {
                                result.Append(pvaAttribute);
                            }
                        }
                    }
                }
            }

            //gift cards
            if (RenderGiftCardAttributes)
            {
                if (productVariant.IsGiftCard)
                {
                    string giftCardRecipientName = string.Empty;
                    string giftCardRecipientEmail = string.Empty;
                    string giftCardSenderName = string.Empty;
                    string giftCardSenderEmail = string.Empty;
                    string giftCardMessage = string.Empty;
                    GetGiftCardAttribute(Attributes, out giftCardRecipientName, out giftCardRecipientEmail,
                        out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

                    if (!String.IsNullOrEmpty(result.ToString()))
                    {
                        result.Append(Serapator);
                    }

                    if (HTMLEncode)
                    {
                        result.Append(HttpUtility.HtmlEncode(string.Format(LocalizationManager.GetLocaleResourceString("GiftCardAttribute.For"), giftCardRecipientName)));
                        result.Append(Serapator);
                        result.Append(HttpUtility.HtmlEncode(string.Format(LocalizationManager.GetLocaleResourceString("GiftCardAttribute.From"), giftCardSenderName)));
                    }
                    else
                    {
                        result.Append(string.Format(LocalizationManager.GetLocaleResourceString("GiftCardAttribute.For"), giftCardRecipientName));
                        result.Append(Serapator);
                        result.Append(string.Format(LocalizationManager.GetLocaleResourceString("GiftCardAttribute.From"), giftCardSenderName));
                    }
                }
            }
            return result.ToString();
        }

        #endregion
    }
}
