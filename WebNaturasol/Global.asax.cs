using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebArribosPlanta
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;
            int error = httpException != null ? httpException.GetHttpCode() : 0;
            Server.ClearError();
            if (error == 505)
            {
                Response.Redirect(String.Format("~/Error/index", exception.Message));
            }
            else if (error == 404)
            {
                Response.Redirect(String.Format("~/Error/error404", exception.Message));
            }
            else
            {
                Response.Redirect(String.Format("~/Error/error505", exception.Message));
            }                
        }
    }
}
