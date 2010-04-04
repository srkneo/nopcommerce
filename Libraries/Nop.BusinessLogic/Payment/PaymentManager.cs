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
using System.Text;
using System.Web.Compilation;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;

namespace NopSolutions.NopCommerce.BusinessLogic.Payment
{
    /// <summary>
    /// Payment manager
    /// </summary>
    public partial class PaymentManager
    {
        #region Methods
        /// <summary>
        /// Process payment
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public static void ProcessPayment(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            if (paymentInfo.OrderTotal == decimal.Zero)
            {
                processPaymentResult.Error = string.Empty;
                processPaymentResult.FullError = string.Empty;
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
            }
            else
            {
                var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(paymentInfo.PaymentMethodID);
                if (paymentMethod == null)
                    throw new NopException("Payment method couldn't be loaded");
                var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
                iPaymentMethod.ProcessPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);
            }
        }

        /// <summary>
        /// Post process payment (payment gateways that require redirecting)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public static string PostProcessPayment(Order order)
        {
            //already paid or order.OrderTotal == decimal.Zero
            if (order.PaymentStatus == PaymentStatusEnum.Paid)
                return string.Empty;

            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.PostProcessPayment(order);
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Additional handling fee</returns>
        public static decimal GetAdditionalHandlingFee(int PaymentMethodID)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return decimal.Zero;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.GetAdditionalHandlingFee();
        }

        /// <summary>
        /// Gets a value indicating whether capture is supported by payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>A value indicating whether capture is supported</returns>
        public static bool CanCapture(int PaymentMethodID)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanCapture;
        }
        
        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public static void Capture(Order order, ref ProcessPaymentResult processPaymentResult)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Capture(order, ref processPaymentResult);
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported by payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>A value indicating whether refund is supported</returns>
        public static bool CanRefund(int PaymentMethodID)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanRefund;
        }

        /// <summary>
        /// Refunds payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        public static void Refund(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Refund(order, ref cancelPaymentResult);
        }

        /// <summary>
        /// Gets a value indicating whether void is supported by payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>A value indicating whether void is supported</returns>
        public static bool CanVoid(int PaymentMethodID)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return false;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.CanVoid;
        }

        /// <summary>
        /// Voids payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        public static void Void(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.Void(order, ref cancelPaymentResult);
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>A recurring payment type of payment method</returns>
        public static RecurringPaymentTypeEnum SupportRecurringPayments(int PaymentMethodID)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return RecurringPaymentTypeEnum.NotSupported;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.SupportRecurringPayments;
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>A payment method type</returns>
        public static PaymentMethodTypeEnum GetPaymentMethodType(int PaymentMethodID)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(PaymentMethodID);
            if (paymentMethod == null)
                return PaymentMethodTypeEnum.Unknown;
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            return iPaymentMethod.PaymentMethodType;
        }

        /// <summary>
        /// Process recurring payments
        /// </summary>
        /// <param name="paymentInfo">Payment info required for an order processing</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderGuid">Unique order identifier</param>
        /// <param name="processPaymentResult">Process payment result</param>
        public static void ProcessRecurringPayment(PaymentInfo paymentInfo, Customer customer, Guid OrderGuid, ref ProcessPaymentResult processPaymentResult)
        {
            if (paymentInfo.OrderTotal == decimal.Zero)
            {
                processPaymentResult.Error = string.Empty;
                processPaymentResult.FullError = string.Empty;
                processPaymentResult.PaymentStatus = PaymentStatusEnum.Paid;
            }
            else
            {
                var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(paymentInfo.PaymentMethodID);
                if (paymentMethod == null)
                    throw new NopException("Payment method couldn't be loaded");
                var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
                iPaymentMethod.ProcessRecurringPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);
            }
        }

        /// <summary>
        /// Cancels recurring payment
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="cancelPaymentResult">Cancel payment result</param>
        public static void CancelRecurringPayment(Order order, ref CancelPaymentResult cancelPaymentResult)
        {
            var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(order.PaymentMethodID);
            if (paymentMethod == null)
                throw new NopException("Payment method couldn't be loaded");
            var iPaymentMethod = Activator.CreateInstance(Type.GetType(paymentMethod.ClassName)) as IPaymentMethod;
            iPaymentMethod.CancelRecurringPayment(order, ref cancelPaymentResult);
        }

        /// <summary>
        /// Gets masked credit card number
        /// </summary>
        /// <param name="CreditCardNumber">Credit card number</param>
        /// <returns>Masked credit card number</returns>
        public static string GetMaskedCreditCardNumber(string CreditCardNumber)
        {
            if (String.IsNullOrEmpty(CreditCardNumber))
                return string.Empty;

            if (CreditCardNumber.Length <= 4)
                return CreditCardNumber;

            string last4 = CreditCardNumber.Substring(CreditCardNumber.Length - 4, 4);
            string maskedChars = string.Empty;
            for (int i = 0; i < CreditCardNumber.Length - 4; i++)
            {
                maskedChars += "*";
            }
            return maskedChars + last4;
        }
        #endregion
    }
}
