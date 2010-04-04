using System;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class NewsLetterSubscriptionActivationControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                var NewsLetterSubscriptionGuid = CommonHelper.QueryStringGUID("T");
                bool IsActive = CommonHelper.QueryStringBool("Active");
                if(!NewsLetterSubscriptionGuid.HasValue)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                var subscription = MessageManager.GetNewsLetterSubscriptionByGUID(NewsLetterSubscriptionGuid.Value);
                if(subscription == null)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                subscription = MessageManager.UpdateNewsLetterSubscription(subscription.NewsLetterSubscriptionID, subscription.Email, IsActive);

                if(subscription.IsActive)
                {
                    lblActivationResult.Text = GetLocaleResourceString("NewsLetterSubscriptionActivation.ResultActivated");
                }
                else
                {
                    lblActivationResult.Text = GetLocaleResourceString("NewsLetterSubscriptionActivation.ResultDectivated");
                }
            }
        }
    }
}