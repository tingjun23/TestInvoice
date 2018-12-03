using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using TestInvoice.DataAccess.EntityFramework;
using TestInvoice.DataAccess.Repositories;
using TestInvoice.DataAccess.Repositories.Interfaces;
using TestInvoice.Service.Services;
using TestInvoice.Service.Services.Interfaces;

namespace TestInvoice.WebApi
{
    public class AutoFacConfig
    {
        public static void RegisterAutoFac()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            builder.RegisterType<InvoiceDbContext>().InstancePerRequest();

            builder.RegisterType<ClientRepository>().As<IClientRepository>().InstancePerRequest();
            builder.RegisterType<GoodAndServiceRepository>().As<IGoodAndServiceRepository>().InstancePerRequest();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest();
            builder.RegisterType<InvoiceRepository>().As<IInvoiceRepository>().InstancePerRequest();

            builder.RegisterType<InvoiceService>().As<IInvoiceService>().InstancePerRequest();
            builder.RegisterType<ClientService>().As<IClientService>().InstancePerRequest();
            builder.RegisterType<GoodAndServiceService>().As<IGoodAndServiceService>().InstancePerRequest();

            //automapper
            //register your profiles, or skip this if you don't want them in your container
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "TestInvoice.Service")).As<Profile>();

            //register your configuration as a single instance
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                //add your profiles (either resolve from container or however else you acquire them)
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            //register your mapper
            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve)).As<IMapper>().InstancePerLifetimeScope();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}