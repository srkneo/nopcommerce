using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Payment.Methods.Svea;

namespace NopSolutions.NopCommerce.Web
{
    public partial class SveaHostedPaymentReturn : BaseNopPage
    {
        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if((NopContext.Current.User == null) || NopContext.Current.User.IsGuest)
            {
                Response.Redirect(SEOHelper.GetLoginPageURL(true));
            }

            CommonHelper.SetResponseNoCache(Response);

            if(!Page.IsPostBack)
            {

                Order order = OrderManager.GetOrderByID(CommonHelper.QueryStringInt("OrderID"));
                if(order == null)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }
                if(NopContext.Current.User.CustomerID != order.CustomerID)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                string md5 = CommonHelper.QueryString("MD5");

                if(String.IsNullOrEmpty(md5) || !md5.Equals(HostedPaymentHelper.CalcMd5Hash(String.Format("{0}SveaHostedPaymentReturn.aspx{1}{2}", CommonHelper.GetStoreHost(false), Regex.Replace(Request.Url.Query, "&MD5=.*", String.Empty), HostedPaymentSettings.Password))))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                if (OrderManager.CanMarkOrderAsPaid(order))
                {
                    OrderManager.MarkOrderAsPaid(order.OrderID);
                }

                Response.Redirect("~/checkoutcompleted.aspx");
            }
        }
        #endregion
    }
}
