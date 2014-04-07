using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace EDFServer
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

			routes.MapPageRoute("user-apps",
				"user/{uid}",
				"~/User.aspx");
			routes.MapPageRoute("user-usage",
				"user/{uid}/usage",
				"~/Usage.aspx");
			routes.MapPageRoute("module-usage",
				"module/{mid}/usage",
				"~/Usage.aspx");
			routes.EnableFriendlyUrls();
        }
    }
}