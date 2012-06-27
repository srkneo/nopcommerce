using System.Collections.Generic;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.GoogleAnalytics
{
    /// <summary>
    /// Live person provider
    /// </summary>
    public class GoogleAnalyticPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly GoogleAnalyticsSettings _googleAnalyticsSettings;

        public GoogleAnalyticPlugin(ISettingService settingService,
            GoogleAnalyticsSettings googleAnalyticsSettings)
        {
            this._settingService = settingService;
            this._googleAnalyticsSettings = googleAnalyticsSettings;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return !string.IsNullOrWhiteSpace(_googleAnalyticsSettings.WidgetZone)
                       ? new List<string>() { _googleAnalyticsSettings.WidgetZone }
                       : new List<string>() { "head_html_tag" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsGoogleAnalytics";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.GoogleAnalytics.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsGoogleAnalytics";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.GoogleAnalytics.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new GoogleAnalyticsSettings()
            {
                GoogleId = "UA-0000000-0",
                //TrackingScript = "<script type=\"text/javascript\"> var gaJsHost = ((\"https:\" == document.location.protocol) ? \"https://ssl.\" : \"http://www.\"); document.write(unescape(\"%3Cscript src='\" + gaJsHost + \"google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E\")); </script> <script type=\"text/javascript\"> try { var pageTracker = _gat._getTracker(\"UA-0000000-0\"); pageTracker._trackPageview(); } catch(err) {}</script>",
                TrackingScript = @"<!-- Google code for Analytics tracking -->
<script type=""text/javascript"">
var _gaq = _gaq || [];
_gaq.push(['_setAccount', '{GOOGLEID}']);
_gaq.push(['_trackPageview']);
{ECOMMERCE}
(function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();
</script>",
                EcommerceScript = @"_gaq.push(['_addTrans', '{ORDERID}', '{SITE}', '{TOTAL}', '{TAX}', '{SHIP}', '{CITY}', '{STATEPROVINCE}', '{COUNTRY}']);
{DETAILS} 
_gaq.push(['_trackTrans']); ",
                EcommerceDetailScript = @"_gaq.push(['_addItem', '{ORDERID}', '{PRODUCTSKU}', '{PRODUCTNAME}', '{CATEGORYNAME}', '{UNITPRICE}', '{QUANTITY}' ]); ",

            };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId", "ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId.Hint", "Enter Google Analytics ID.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript", "Tracking code with {ECOMMERCE} line");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript.Hint", "Paste the tracking code generated by Google Analytics here. {GOOGLEID} and {ECOMMERCE} will be dynamically replaced.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript", "Tracking code for {ECOMMERCE} part, with {DETAILS} line");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript.Hint", "Paste the tracking code generated by Google analytics here. {ORDERID}, {SITE}, {TOTAL}, {TAX}, {SHIP}, {CITY}, {STATEPROVINCE}, {COUNTRY}, {DETAILS} will be dynamically replaced.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript", "Tracking code for {DETAILS} part");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript.Hint", "Paste the tracking code generated by Google analytics here. {ORDERID}, {PRODUCTSKU}, {PRODUCTNAME}, {CATEGORYNAME}, {UNITPRICE}, {QUANTITY} will be dynamically replaced.");
            
            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<GoogleAnalyticsSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceScript.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EcommerceDetailScript.Hint");
            
            base.Uninstall();
        }
    }
}
