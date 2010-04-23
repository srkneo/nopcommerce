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
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Payment.Methods.QuickPay
{
    /// <summary>
    /// QuickPay payment processor
    /// </summary>
    public class QuickPayPaymentProcessor : IPaymentMethod
    {
        #region Methods

        public string GetMD5(string InputStr)
        {
            byte[] textBytes = Encoding.Default.GetBytes(InputStr);
            try
            {
                MD5CryptoServiceProvider cryptHandler = new MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            processPaymentResult.PaymentStatus = PaymentStatusEnum.Pending;
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public string PostProcessPayment(Order order)
        {
            CultureInfo cultureInfo = new CultureInfo(NopContext.Current.WorkingLanguage.LanguageCulture);

            string language = cultureInfo.TwoLetterISOLanguageName;            
            string amount = (order.OrderTotal * 100).ToString("0", CultureInfo.InvariantCulture);

            string currencyCode = CurrencyManager.PrimaryStoreCurrency.CurrencyCode;
            string protocol = "3";
            string autocapture = "0";
            string cardtypelock = SettingManager.GetSettingValue(QuickPayConstants.SETTING_CREDITCARD_CODE_PROPERTY);
            bool useSandBox = SettingManager.GetSettingValueBoolean(QuickPayConstants.SETTING_USE_SANDBOX);
            string testmode = (useSandBox) ? "1" : "0";
            string continueurl = CommonHelper.GetStoreLocation(false) + "CheckoutCompleted.aspx";
            string cancelurl = CommonHelper.GetStoreLocation(false) + "QuickpayCancel.aspx";
            string callbackurl = CommonHelper.GetStoreLocation(false) + "QuickpayReturn.aspx";
            string merchant = SettingManager.GetSettingValue(QuickPayConstants.SETTING_MERCHANTID);
            string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            string msgtype = "authorize";
            string md5secret = SettingManager.GetSettingValue(QuickPayConstants.SETTING_MD5SECRET);
            string ordernumber = order.OrderId.ToString();

            //order number must be at least 4 digits long.
            if (ordernumber.Length < 4)
            {
                for (int i = 1; i < 4; i++)
                {
                    if (ordernumber.Length < 4)
                    {
                        ordernumber = "0" + ordernumber;
                    }
                }
            }

            string stringToMd5 = string.Concat(protocol, msgtype, merchant, language, ordernumber, amount, currencyCode, continueurl, cancelurl, callbackurl, autocapture, cardtypelock, ipaddress, testmode, md5secret);
            string md5check = GetMD5(stringToMd5);

            RemotePost remotePostHelper = new RemotePost();
            remotePostHelper.FormName = "QuickPay";
            remotePostHelper.Url = "https://secure.quickpay.dk/form/";
            remotePostHelper.Add("protocol", protocol);
            remotePostHelper.Add("msgtype", msgtype);
            remotePostHelper.Add("merchant", merchant);
            remotePostHelper.Add("language", language);
            remotePostHelper.Add("ordernumber", ordernumber);
            remotePostHelper.Add("amount", amount);
            remotePostHelper.Add("currency", currencyCode);
            remotePostHelper.Add("continueurl", continueurl);
            remotePostHelper.Add("cancelurl", cancelurl);
            remotePostHelper.Add("callbackurl", callbackurl);
            remotePostHelper.Add("autocapture", autocapture);
            remotePostHelper.Add("cardtypelock", cardtypelock);
            remotePostHelper.Add("testmode", testmode);
            remotePostHelper.Add("ipaddress", ipaddress);
            remotePostHelper.Add("md5check", md5check);


            remotePostHelper.Post();
            return string.Empty;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee()
        {
            return SettingManager.GetSettingValueDecimalNative("PaymentMethod.QuickPay.AdditionalFee");
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NopException("Capture method not supported");
        }

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Refund(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NopException("Refund method not supported");
        }

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void Void(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            throw new NopException("Void method not supported");
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="orderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public void ProcessRecurringPayment(PaymentInfo paymentInfo, Customer customer, Guid orderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            throw new NopException("Recurring payments not supported");
        }

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>        
        public void CancelRecurringPayment(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
        }
        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool CanCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool CanRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool CanVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        /// <returns>A recurring payment type of payment method</returns>
        public RecurringPaymentTypeEnum SupportRecurringPayments
        {
            get
            {
                return RecurringPaymentTypeEnum.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <returns>A payment method type</returns>
        public PaymentMethodTypeEnum PaymentMethodType
        {
            get
            {
                return PaymentMethodTypeEnum.Standard;
            }
        }
        #endregion
    }
}
