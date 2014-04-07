<%@ Application Language="C#" %>
<%@ Import Namespace="EDFServer" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
		log4net.Config.XmlConfigurator.Configure();
        // Code that runs on application startup
        AuthConfig.RegisterOpenAuth();
        RouteConfig.RegisterRoutes(RouteTable.Routes);
    }
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
		var err = Server.GetLastError();
		LogManager.Log.Error(err);
    }

</script>
