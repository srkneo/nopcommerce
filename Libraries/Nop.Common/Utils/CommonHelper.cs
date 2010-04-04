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
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using NopSolutions.NopCommerce.Common.Utils.Html;

namespace NopSolutions.NopCommerce.Common.Utils
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {
        #region Methods
        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="Email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string Email)
        {
            bool result = false;
            if (String.IsNullOrEmpty(Email))
                return result;
            Email = Email.Trim();
            result = Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return result;
        }

        /// <summary>
        /// Gets query string value by name
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static string QueryString(string Name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString[Name] != null)
                result = HttpContext.Current.Request.QueryString[Name].ToString();
            return result;
        }

        /// <summary>
        /// Gets boolean value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static bool QueryStringBool(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            return (resultStr == "YES" || resultStr == "TRUE" || resultStr == "1");
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            int result;
            Int32.TryParse(resultStr, out result);
            return result;
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string Name, int DefaultValue)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            if (resultStr.Length > 0)
            {
                return Int32.Parse(resultStr);
            }
            return DefaultValue;
        }

        /// <summary>
        /// Gets GUID value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static Guid? QueryStringGUID(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            Guid? result = null;
            try
            {
                result = new Guid(resultStr);
            }
            catch
            {
            }
            return result;
        }
        
        /// <summary>
        /// Gets Form String
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string GetFormString(string Name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request[Name] != null)
                result = HttpContext.Current.Request[Name].ToString();
            return result;
        }

        /// <summary>
        /// Set meta http equiv (eg. redirects)
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="httpEquiv">Http Equiv</param>
        /// <param name="Content">Content</param>
        public static void SetMetaHttpEquiv(Page page, string httpEquiv, string Content)
        {
            if (page.Header == null)
                return;

            HtmlMeta meta = new HtmlMeta();
            if (page.Header.FindControl("meta" + httpEquiv) != null)
            {
                meta = (HtmlMeta)page.Header.FindControl("meta" + httpEquiv);
                meta.Content = Content;
            }
            else
            {
                meta.ID = "meta" + httpEquiv;
                meta.HttpEquiv = httpEquiv;
                meta.Content = Content;
                page.Header.Controls.Add(meta);
            }
        }

        /// <summary>
        /// Selects item
        /// </summary>
        /// <param name="List">List</param>
        /// <param name="Value">Value to select</param>
        public static void SelectListItem(DropDownList List, object Value)
        {
            if (List.Items.Count != 0)
            {
                var selectedItem = List.SelectedItem;
                if (selectedItem != null)
                    selectedItem.Selected = false;
                if (Value != null)
                {
                    selectedItem = List.Items.FindByValue(Value.ToString());
                    if (selectedItem != null)
                        selectedItem.Selected = true;
                }
            }
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string Name)
        {
            string tmpS = String.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables[Name] != null)
                {
                    tmpS = HttpContext.Current.Request.ServerVariables[Name].ToString();
                }
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        /// <summary>
        /// Gets a value indicating whether requested page is an admin page
        /// </summary>
        /// <returns>A value indicating whether requested page is an admin page</returns>
        public static bool IsAdmin()
        {
            string thisPageURL = GetThisPageURL(false);
            if (string.IsNullOrEmpty(thisPageURL))
                return false;

            string adminUrl1 = GetStoreLocation(false) + "administration";
            string adminUrl2 = GetStoreLocation(true) + "administration";            
            bool flag1 = thisPageURL.ToLowerInvariant().StartsWith(adminUrl1.ToLower());
            bool flag2 = thisPageURL.ToLowerInvariant().StartsWith(adminUrl2.ToLower());
            bool isAdmin = flag1 || flag2;
            return isAdmin;
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public static bool IsCurrentConnectionSecured()
        {
            bool useSSL = false;
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                useSSL = HttpContext.Current.Request.IsSecureConnection;
                //when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                //just uncomment it
                //useSSL = HttpContext.Current.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSSL;
        }

        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <returns></returns>
        public static string GetThisPageURL(bool includeQueryString)
        {
            string URL = string.Empty;
            if (HttpContext.Current == null)
                return URL;

            if (includeQueryString)
            {
                bool useSSL = IsCurrentConnectionSecured();
                string storeHost = GetStoreHost(useSSL);
                if (storeHost.EndsWith("/"))
                    storeHost = storeHost.Substring(0, storeHost.Length - 1);
                URL = storeHost + HttpContext.Current.Request.RawUrl;
            }
            else
            {
                URL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            }
            return URL;
        }

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        public static string GetStoreLocation()
        {
            bool useSSL = IsCurrentConnectionSecured();
            return GetStoreLocation(useSSL);
        }

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        /// <returns>Store location</returns>
        public static string GetStoreLocation(bool UseSSL)
        {
            string result = GetStoreHost(UseSSL);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            result = result + HttpContext.Current.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        /// <summary>
        /// Gets store admin location
        /// </summary>
        /// <returns>Store admin location</returns>
        public static string GetStoreAdminLocation()
        {
            bool useSSL = IsCurrentConnectionSecured();
            return GetStoreAdminLocation(useSSL);
        }

        /// <summary>
        /// Gets store admin location
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        /// <returns>Store admin location</returns>
        public static string GetStoreAdminLocation(bool UseSSL)
        {
            string result = GetStoreLocation(UseSSL);
            result += "Administration/";

            return result;
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        /// <returns>Store host location</returns>
        public static string GetStoreHost(bool UseSSL)
        {
            string result = "http://" + ServerVariables("HTTP_HOST");
            if (!result.EndsWith("/"))
                result += "/";

            if (UseSSL)
            {
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SharedSSL"]))
                {
                    result = ConfigurationManager.AppSettings["SharedSSL"];
                }
                else
                {
                    result = result.Replace("http:/", "https:/");
                }
            }

            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        public static void ReloadCurrentPage()
        {
            bool useSSL = IsCurrentConnectionSecured();
            ReloadCurrentPage(useSSL);
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        public static void ReloadCurrentPage(bool UseSSL)
        {
            string storeHost = GetStoreHost(UseSSL);
            if (storeHost.EndsWith("/"))
                storeHost = storeHost.Substring(0, storeHost.Length - 1);
            string URL = storeHost + HttpContext.Current.Request.RawUrl;
            HttpContext.Current.Response.Redirect(URL);
        }

        /// <summary>
        /// Modifies query string
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryStringModification">Query string modification</param>
        /// <param name="targetLocationModification">Target location modification</param>
        /// <returns>New url</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string targetLocationModification)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(targetLocationModification))
            {
                str2 = targetLocationModification;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2)));
        }

        /// <summary>
        /// Remove query string from url
        /// </summary>
        /// <param name="url">Url to modify</param>
        /// <param name="queryString">Query string to remove</param>
        /// <returns>New url</returns>
        public static string RemoveQueryString(string url, string queryString)
        {
            string str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    dictionary.Remove(queryString);

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        /// <summary>
        /// Ensures that requested page is secured (https://)
        /// </summary>
        public static void EnsureSSL()
        {
            if (!IsCurrentConnectionSecured())
            {
                bool useSSL = false;
                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseSSL"]))
                    useSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSL"]);
                if (useSSL)
                {
                    //if (!HttpContext.Current.Request.Url.IsLoopback)
                    //{
                        ReloadCurrentPage(true);
                    //}
                }
            }
        }

        /// <summary>
        /// Ensures that requested page is not secured (http://)
        /// </summary>
        public static void EnsureNonSSL()
        {
            if (IsCurrentConnectionSecured())
            {
                ReloadCurrentPage(false);
            }
        }

        /// <summary>
        /// Sets cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="cookieValue">Cookie value</param>
        /// <param name="ts">Timespan</param>
        public static void SetCookie(String cookieName, string cookieValue, TimeSpan ts)
        {
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Value = HttpContext.Current.Server.UrlEncode(cookieValue);
                DateTime dt = DateTime.Now;
                cookie.Expires = dt.Add(ts);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        /// <summary>
        /// Gets cookie string
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="decode">Decode cookie</param>
        /// <returns>Cookie string</returns>
        public static String GetCookieString(String cookieName, bool decode)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null)
            {
                return String.Empty;
            }
            try
            {
                string tmp = HttpContext.Current.Request.Cookies[cookieName].Value.ToString();
                if (decode)
                    tmp = HttpContext.Current.Server.UrlDecode(tmp);
                return tmp;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets boolean value from cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns>Result</returns>
        public static bool GetCookieBool(String cookieName)
        {
            string str1 = GetCookieString(cookieName, true).ToUpperInvariant();
            return (str1 == "TRUE" || str1 == "YES" || str1 == "1");
        }

        /// <summary>
        /// Gets integer value from cookie
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns>Result</returns>
        public static int GetCookieInt(String cookieName)
        {
            string str1 = GetCookieString(cookieName, true);
            if (!String.IsNullOrEmpty(str1))
                return Convert.ToInt32(str1);
            else
                return 0;
        }

        /// <summary>
        /// Gets boolean value from NameValue collection
        /// </summary>
        /// <param name="config">NameValue collection</param>
        /// <param name="valueName">Name</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Result</returns>
        public static bool ConfigGetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
        {
            bool result;
            string str1 = config[valueName];
            if (str1 == null)
                return defaultValue;
            if (!bool.TryParse(str1, out result))
                throw new NopException(string.Format("Value must be boolean {0}", valueName));
            return result;
        }

        /// <summary>
        /// Gets integer value from NameValue collection
        /// </summary>
        /// <param name="config">NameValue collection</param>
        /// <param name="valueName">Name</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="zeroAllowed">Zero allowed</param>
        /// <param name="maxValueAllowed">Max value allowed</param>
        /// <returns>Result</returns>
        public static int ConfigGetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            int result;
            string str1 = config[valueName];
            if (str1 == null)
                return defaultValue;
            if (!int.TryParse(str1, out result))
            {
                if (zeroAllowed)
                {
                    throw new NopException(string.Format("Value must be non negative integer {0}", valueName));
                }
                throw new NopException(string.Format("Value must be positive integer {0}", valueName));
            }
            if (zeroAllowed && (result < 0))
                throw new NopException(string.Format("Value must be non negative integer {0}", valueName));
            if (!zeroAllowed && (result <= 0))
                throw new NopException(string.Format("Value must be positive integer {0}", valueName));
            if ((maxValueAllowed > 0) && (result > maxValueAllowed))
                throw new NopException(string.Format("Value too big {0}", valueName));
            return result;
        }

        /// <summary>
        /// Write XML to response
        /// </summary>
        /// <param name="xml">XML</param>
        /// <param name="Filename">Filename</param>
        public static void WriteResponseXML(string xml, string Filename)
        {
            if(!String.IsNullOrEmpty(xml))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlDeclaration decl = document.FirstChild as XmlDeclaration;
                if(decl != null)
                {
                    decl.Encoding = "utf-8";
                }
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xml";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", Filename));
                response.BinaryWrite(Encoding.UTF8.GetBytes(document.InnerXml));
                response.End();
            }
        }

        /// <summary>
        /// Write text to response
        /// </summary>
        /// <param name="txt">text</param>
        /// <param name="Filename">Filename</param>
        public static void WriteResponseTXT(string txt, string Filename)
        {
            if (!String.IsNullOrEmpty(txt))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/plain";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", Filename));
                response.BinaryWrite(Encoding.UTF8.GetBytes(txt));
                response.End();
            }
        }

        /// <summary>
        /// Write XLS file to response
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="targetFileName">Target file name</param>
        public static void WriteResponseXLS(string filePath, string targetFileName)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xls";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }
        
        /// <summary>
        /// Write PDF file to response
        /// </summary>
        /// <param name="filePath">File napathme</param>
        /// <param name="targetFileName">Target file name</param>
        /// <remarks>For BeatyStore project</remarks>
        public static void WriteResponsePDF(string filePath, string targetFileName)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/pdf";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", targetFileName));
                response.BinaryWrite(File.ReadAllBytes(filePath));
                response.End();
            }
        }
        
        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="Length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int Length)
        {
            var random = new Random();
            string s = "";
            for (int i = 0; i < Length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>Covnerted string</returns>
        public static string ConvertEnum(string s)
        {
            string result = string.Empty;
            char[] letters = s.ToCharArray();
            foreach (char c in letters)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();
            return result;
        }

        /// <summary>
        /// Fills drop down list with values of enumaration
        /// </summary>
        /// <param name="List">Dropdownlist</param>
        /// <param name="enumType">Enumeration</param>
        public static void FillDropDownWithEnum(DropDownList List, Type enumType)
        {
            if (List == null)
            {
                throw new ArgumentNullException("List");
            }
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType must be enum type");
            }

            List.Items.Clear();
            string[] strArray = Enum.GetNames(enumType);
            foreach (string str2 in strArray)
            {
                int enumValue = (int)Enum.Parse(enumType, str2, true);
                ListItem ddlItem = new ListItem(CommonHelper.ConvertEnum(str2), enumValue.ToString());
                List.Items.Add(ddlItem);
            }
        }
        
        #endregion
    }
}
