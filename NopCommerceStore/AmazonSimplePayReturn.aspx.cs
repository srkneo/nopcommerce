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
using NopSolutions.NopCommerce.Payment.Methods.Amazon;

namespace NopSolutions.NopCommerce.Web
{
    public partial class AmazonSimplePayReturn : BaseNopPage
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

                if(!AmazonHelper.ValidateRequest(Request.QueryString, String.Format("{0}AmazonSimplePayReturn.aspx", CommonHelper.GetStoreLocation()), "GET"))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                int OrderID = Convert.ToInt32(CommonHelper.QueryStringInt("referenceId"));
                Order order = OrderManager.GetOrderByID(OrderID);
                if(order == null)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }
                if(NopContext.Current.User.CustomerID != order.CustomerID)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                if (SimplePaySettings.SettleImmediately)
                {
                    if (OrderManager.CanMarkOrderAsPaid(order))
                    {
                        OrderManager.MarkOrderAsPaid(order.OrderID);
                    }
                }
                else
                {
                    if (OrderManager.CanMarkOrderAsAuthorized(order))
                    {
                        OrderManager.MarkAsAuthorized(order.OrderID);
                    }
                }

                Response.Redirect("~/CheckoutCompleted.aspx");
            }
        }
        #endregion    
    }
}
