using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using com.dcs.common;

namespace com.dcs.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 依赖注入
            var builder = new ContainerBuilder();
            SetupResolveRules(builder);
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Configs/log4net.config")));
            ConfigManager.LoadXmlConfig(Server.MapPath("~/Configs/init.config"));
        }

        private void SetupResolveRules(ContainerBuilder builder)
        {

        }
    }
}
