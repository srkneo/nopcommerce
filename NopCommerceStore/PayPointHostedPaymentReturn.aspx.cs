using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Payment.Methods.PayPoint;

namespace NopSolutions.NopCommerce.Web
{
    public partial class PayPointHostedPaymentReturn : BaseNopPage
    {
        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if(NopContext.Current.User == null)
            {
                string loginURL = SEOHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            if(!Page.IsPostBack)
            {
                Response.CacheControl = "private";
                Response.Expires = 0;
                Response.AddHeader("pragma", "no-cache");

                if(!PayPointHelper.ValidateResponseSign(Request.Url))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }
                if(!CommonHelper.QueryStringBool("valid"))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                int OrderID = CommonHelper.QueryStringInt("trans_id");
                Order order = OrderManager.GetOrderByID(OrderID);
                if(order == null)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }
                if(NopContext.Current.User.CustomerID != order.CustomerID)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                if (OrderManager.CanMarkOrderAsPaid(order))
                {
                    OrderManager.MarkOrderAsPaid(order.OrderID);
                }
                Response.Redirect("~/CheckoutCompleted.aspx");
            }
        }
        #endregion
    }
}
