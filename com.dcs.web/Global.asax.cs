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
using com.dcs.bll;
using com.dcs.ibll;
using com.dcs.dal;
using com.dcs.idal;
using com.dcs.web.Globals;

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
            RolesManager.LoadXmlConfig(Server.MapPath("~/Configs/roles.config"));
        }

        private void SetupResolveRules(ContainerBuilder builder)
        {
            builder.RegisterType<MemberBLL>().As<IMemberBLL>();
            builder.RegisterType<MemberDAL>().As<IMemberDAL>();
            builder.RegisterType<CustomItemBLL>().As<ICustomItemBLL>();
            builder.RegisterType<CustomItemDAL>().As<ICustomItemDAL>();
            builder.RegisterType<InformationBLL>().As<IInformationBLL>();
            builder.RegisterType<InformationDAL>().As<IInformationDAL>();
            builder.RegisterType<CompanyBLL>().As<ICompanyBLL>();
            builder.RegisterType<CompanyDAL>().As<ICompanyDAL>();
        }
    }
}
