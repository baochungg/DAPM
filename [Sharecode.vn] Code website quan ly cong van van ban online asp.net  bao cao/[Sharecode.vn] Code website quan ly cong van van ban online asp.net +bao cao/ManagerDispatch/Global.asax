<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
    }
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        //department
        //UrlRewritingNet.Web.RegExRewriteRule submenu = new UrlRewritingNet.Web.RegExRewriteRule();
        //submenu.VirtualUrl = "^/ManagerDispatch/(.*)/department";
        //submenu.DestinationUrl = "~/Department.aspx?parentMenuID=$1";
        //submenu.IgnoreCase = true;
        //submenu.Rewrite = UrlRewritingNet.Web.RewriteOption.Application;
        //submenu.Redirect = UrlRewritingNet.Web.RedirectOption.None;
        //submenu.RewriteUrlParameter = UrlRewritingNet.Web.RewriteUrlParameterOption.ExcludeFromClientQueryString;
        //UrlRewritingNet.Web.UrlRewriting.AddRewriteRule("Main_SubMenu", submenu);
        
        //main-menu
        //UrlRewritingNet.Web.RegExRewriteRule defaultPage = new UrlRewritingNet.Web.RegExRewriteRule();
        //defaultPage.VirtualUrl = "^/ManagerDispatch/(.*)";
        //defaultPage.DestinationUrl = "~/Default.aspx?MainMenuID=$1";
        //defaultPage.IgnoreCase = true;
        //defaultPage.Rewrite = UrlRewritingNet.Web.RewriteOption.Application;
        //defaultPage.Redirect = UrlRewritingNet.Web.RedirectOption.None;
        //defaultPage.RewriteUrlParameter = UrlRewritingNet.Web.RewriteUrlParameterOption.ExcludeFromClientQueryString;
        //UrlRewritingNet.Web.UrlRewriting.AddRewriteRule("Main", defaultPage);

      
        
    }
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
        
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        if (Session["ACOUNT"] == null)
        {
            Response.Redirect("/ManagerDispatch/Login.aspx");
        }
    }

    void Session_End(object sender, EventArgs e) 
    {
        if (Session["FileAttachment"] != null)
        {
            System.Collections.Generic.List<FileAttSession> fas = Session["FileAttachment"] as System.Collections.Generic.List<FileAttSession>;
            if (fas != null)
            {
                MDFileAttachment md_FABus = new MDFileAttachment();
                for (int i = 0; i < fas.Count; i++)
                {
                    foreach (string key in fas[i].FileAttIDs.Keys)
                    {
                        md_FABus.DeleteFile(key);
                    }
                }
            }
        }
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
