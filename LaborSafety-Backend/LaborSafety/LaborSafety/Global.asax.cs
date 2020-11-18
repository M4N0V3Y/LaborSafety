using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LaborSafety.App_Start;
using LaborSafety.Negocio.Mapeamento;
using System.Data.Entity.SqlServer;
using Unity;
using System;
using EntregaEColeta.API.App_Start;

namespace LaborSafety
{

    public class WebApiApplication : System.Web.HttpApplication, IContainerAccessor
    {
        private static IUnityContainer _container;
        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
            private set
            {
                _container = value;
            }
        }

        IUnityContainer IContainerAccessor.Container
        {
            get
            {
                return Container;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Inicializa AutoMapper
            Mapper.Run();

            // Registra depend?ncias
            var container = new UnityContainer();
            UnityConfig.RegisterTypes(container);

            Container = container;

            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            SqlProviderServices.SqlServerTypesAssemblyName = "Microsoft.SqlServer.Types, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

            GlobalConfiguration.Configuration.Filters.Add(new AprExceptionFilterAttribute());
        }
    }
}
