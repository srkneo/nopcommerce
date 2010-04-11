using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Payment.Methods.PayPoint;

namespace NopSolutions.NopCommerce.Web
{
    public partial class SermepaReturnPage : BaseNopPage
    {
        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonHelper.SetResponseNoCache(Response);

            if (!Page.IsPostBack)
            {
                string HostName = Request.UserHostName;
                LogManager.InsertLog(LogTypeEnum.OrderError, "TPV SERMEPA: Host " + HostName, "Host: " + HostName);

                //ID de Pedido
                string orderID = Request["Ds_Order"];
                string strDs_Merchant_Order = Request["Ds_Order"];

                string strDs_Merchant_Amount = Request["Ds_Amount"];
                string strDs_Merchant_MerchantCode = Request["Ds_MerchantCode"];
                string strDs_Merchant_Currency = Request["Ds_Currency"];

                //Respuesta del TPV
                string str_Merchant_Response = Request["Ds_Response"];
                int Ds_Response = Convert.ToInt32(Request["Ds_Response"]);

                //Clave
                bool pruebas = SettingManager.GetSettingValueBoolean("PaymentMethod.Sermepa.Pruebas");
                string clave = "";
                if (pruebas) { clave = SettingManager.GetSettingValue("PaymentMethod.Sermepa.ClavePruebas"); }
                else { clave = SettingManager.GetSettingValue("PaymentMethod.Sermepa.ClaveReal"); };

                //Calculo de la firma
                string SHA = string.Format("{0}{1}{2}{3}{4}{5}",
                    strDs_Merchant_Amount,
                    strDs_Merchant_Order,
                    strDs_Merchant_MerchantCode,
                    strDs_Merchant_Currency,
                    str_Merchant_Response,
                    clave);

                byte[] SHAresult;
                SHA1 shaM = new SHA1Managed();
                SHAresult = shaM.ComputeHash(Encoding.Default.GetBytes(SHA));
                string SHAresultStr = BitConverter.ToString(SHAresult).Replace("-", "");

                //Firma enviada
                string signature = Request["Ds_Signature"];

                //Comprobamos la integridad de las comunicaciones con las claves
                //LogManager.InsertLog(LogTypeEnum.OrderError, "TPV SERMEPA: Clave generada", "CLAVE GENERADA: " + SHAresultStr);
                //LogManager.InsertLog(LogTypeEnum.OrderError, "TPV SERMEPA: Clave obtenida", "CLAVE OBTENIDA: " + signature);
                if (!signature.Equals(SHAresultStr))
                {
                    LogManager.InsertLog(LogTypeEnum.OrderError, "TPV SERMEPA: Clave incorrecta", "Las claves enviada y generada no coinciden: " + SHAresultStr + " != " + signature);
                    return;
                }

                //Pedido
                Order order = OrderManager.GetOrderByID(Convert.ToInt32(orderID));
                if (order == null)
                    throw new NopException(string.Format("El pedido de ID {0} no existe", orderID));

                //Actualizamos el pedido
                if (Ds_Response > -1 && Ds_Response < 100)
                {
                    //Lo marcamos como pagado
                    if (OrderManager.CanMarkOrderAsPaid(order))
                    {
                        OrderManager.MarkOrderAsPaid(order.OrderID);
                    }
                    //OrderManager.InsertOrderNote(order.OrderID, "Información del pago: " + Request.Form.ToString(), DateTime.Now);
                }
                else
                {
                    LogManager.InsertLog(LogTypeEnum.OrderError, "TPV SERMEPA: Pago no autorizado", "Pago no autorizado con ERROR: " + Ds_Response);
                    //OrderManager.InsertOrderNote(order.OrderID, "!!! PAGO DENEGADO !!! " + Request.Form.ToString(), DateTime.Now);
                }

            }
        }
        #endregion
    }
}
