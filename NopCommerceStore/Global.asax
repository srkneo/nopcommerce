<%@ Application Language="C#" %>
<%@ Import Namespace="NopSolutions.NopCommerce.BusinessLogic.Configuration" %>
<%@ Import Namespace="NopSolutions.NopCommerce.BusinessLogic" %>
<%@ Import Namespace="NopSolutions.NopCommerce.BusinessLogic.Installation" %>
<%@ Import Namespace="NopSolutions.NopCommerce.BusinessLogic.Utils" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">

    void Application_BeginRequest(object sender, EventArgs e)
    {
        NopConfig.Init();
        if (!InstallerHelper.ConnectionStringIsSet())
        {
            InstallerHelper.InstallRedirect();
        }
    }


    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        NopConfig.Init();
        if (InstallerHelper.ConnectionStringIsSet())
        {
            TaskManager.Instance.Initialize(NopConfig.ScheduleTasks);
            TaskManager.Instance.Start();
        }
    }
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        if (InstallerHelper.ConnectionStringIsSet())
        {
            TaskManager.Instance.Stop();
        }
    }
    
    void Application_Error(object sender, EventArgs e)
    {

        Exception ex = Server.GetLastError();
        if (ex != null)
        {
            //try
            //{
                if (InstallerHelper.ConnectionStringIsSet())
                {
                    LogManager.InsertLog(LogTypeEnum.Unknown, ex.Message, ex);
                }
            //}
            //catch
            //{
                //TODO write to file
                //if (HttpContext.Current != null)
                //{
                //    string path = "~/Error/" + DateTime.Today.ToString("dd-mm-yy") + ".txt";
                //    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                //    {
                //        File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                //    }
                //    using (StreamWriter w = File.AppendText(HttpContext.Current.Server.MapPath(path)))
                //    {
                //        w.WriteLine("\r\nLog Entry : ");
                //        w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                //        string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                //                      ". Error Message:" + ex.Message;
                //        w.WriteLine(err);
                //        w.WriteLine("__________________________");
                //        w.Flush();
                //        w.Close();
                //    }
                //}
            //}
        }
    }
    
</script>

