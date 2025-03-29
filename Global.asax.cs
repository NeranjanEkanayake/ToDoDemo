using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ToDoApp.Models;

namespace ToDoApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            Database.SetInitializer(new ApplicationDbInitializer());
            Database.SetInitializer <ApplicationDbContext>(null);
        }
    }
}
