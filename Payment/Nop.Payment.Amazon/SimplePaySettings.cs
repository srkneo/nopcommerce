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
// Contributor(s): 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;

namespace NopSolutions.NopCommerce.Payment.Methods.Amazon
{
    public class SimplePaySettings
    {
        #region Properties
        public static string GatewayUrl
        {
            get
            {
                return SettingManager.GetSettingValue("PaymentMethod.Amazon.SimplePay.GatewayUrl", "https://authorize.payments-sandbox.amazon.com/pba/paypipeline");
            }
            set
            {
                SettingManager.SetParam("PaymentMethod.Amazon.SimplePay.GatewayUrl", value);
            }
        }

        public static string AccountId
        {
            get
            {
                return SettingManager.GetSettingValue("PaymentMethod.Amazon.SimplePay.AccountId");
            }
            set
            {
                SettingManager.SetParam("PaymentMethod.Amazon.SimplePay.AccountId", value);
            }
        }

        public static string AccessKey
        {
            get
            {
                return SettingManager.GetSettingValue("PaymentMethod.Amazon.SimplePay.AccessKey");
            }
            set
            {
                SettingManager.SetParam("PaymentMethod.Amazon.SimplePay.AccessKey", value);
            }
        }

        public static string SecretKey
        {
            get
            {
                return SettingManager.GetSettingValue("PaymentMethod.Amazon.SimplePay.SecretKey");
            }
            set
            {
                SettingManager.SetParam("PaymentMethod.Amazon.SimplePay.SecretKey", value);
            }
        }

        public static bool SettleImmediately
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("PaymentMethod.Amazon.SimplePay.SettleImmediately", false);
            }
            set
            {
                SettingManager.SetParam("PaymentMethod.Amazon.SimplePay.SettleImmediately", value.ToString());
            }
        }

        public static decimal AdditionalFee
        {
            get
            {
                return SettingManager.GetSettingValueDecimalNative("PaymentMethod.Amazon.SimplePay.AdditionalFee");
            }
            set
            {
                SettingManager.SetParamNative("PaymentMethod.Amazon.SimplePay.AdditionalFee", value);
            }
        }
        #endregion
    }
}
