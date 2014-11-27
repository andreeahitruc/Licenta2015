using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using FlickrNet;
using AttributeRouting.Web.Http;
using Back_F.Models;

namespace Back_F
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var config = GlobalConfiguration.Configuration;
          //  config.Formatters.Insert(0, new Models.JsonpMediaTypeFormatter());
            Models.User.FlickerObj.UserFl = new Dictionary<string,Flickr>();
        }

    }
}