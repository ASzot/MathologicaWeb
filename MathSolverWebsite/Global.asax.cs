using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using MathSolverWebsite;

namespace MathSolverWebsite
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            String fullOrigionalpath = Request.Url.ToString();
            String[] sElements = fullOrigionalpath.Split('/');
            String[] sFilePath = sElements[sElements.Length - 1].Split('.');

            if (!fullOrigionalpath.Contains(".aspx") && sFilePath.Length == 1)
            {
                if (!string.IsNullOrEmpty(sFilePath[0].Trim()))
                    Context.RewritePath(sFilePath[0] + ".aspx");
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
