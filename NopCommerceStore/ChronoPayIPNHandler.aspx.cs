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
using NopSolutions.NopCommerce.Payment.Methods.ChronoPay;
using NopSolutions.NopCommerce.BusinessLogic.Audit;

namespace NopSolutions.NopCommerce.Web
{
    public partial class ChronoPayIPNHandler : BaseNopPage
    {
        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                Response.CacheControl = "private";
                Response.Expires = 0;
                Response.AddHeader("pragma", "no-cache");

                if(HostedPaymentHelper.ValidateResponseSign(Request.Form))
                {
                    int OrderID = 0;
                    if(Int32.TryParse(Request.Form["cs1"], out OrderID))
                    {
                        Order order = OrderManager.GetOrderByID(OrderID);
                        if(order != null && OrderManager.CanMarkOrderAsPaid(order))
                        {
                            OrderManager.MarkOrderAsPaid(order.OrderID);
                        }
                    }
                }
            }
        }
        #endregion    
    }
}
