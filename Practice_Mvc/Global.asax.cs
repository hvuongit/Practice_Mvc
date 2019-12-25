﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Practice_Mvc.Infrastructure;
using StructureMap;
using StructureMap.TypeRules;

namespace Practice_Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public IContainer Container
        {
            get => (IContainer) HttpContext.Current.Items["_Container"];
            set => HttpContext.Current.Items["_Container"] = value;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(() => Container ?? ObjectFactory.Container));

            ObjectFactory.Configure(cfg =>
            {
                cfg.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });
                cfg.For<IFilterProvider>().Use(
                    new StructureMapFilterProvider(() => Container ?? ObjectFactory.Container));
                
                cfg.SetAllProperties(x =>
                    x.Matching(p =>
                        p.DeclaringType.CanBeCastTo(typeof(ActionFilterAttribute)) &&
                            p.DeclaringType.Namespace.StartsWith("Practice_Mvc") &&
                            !p.PropertyType.IsPrimitive &&
                            p.PropertyType != typeof(string)
                    ));
            });
        }

        public void Application_BeginRequest()
        {
            Container = ObjectFactory.Container.GetNestedContainer();
        }

        public void Application_EndRequest()
        {
            Container.Dispose();
            Container = null;
        }
    }
}
