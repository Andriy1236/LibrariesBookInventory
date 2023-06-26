using Autofac;
using Autofac.Integration.Web;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using LibrariesBookInventory.Persistence;
using LibrariesBookInventory.Application;
using LibrariesBookInventory.DomainServices.Implementation;
using LibrariesBookInventory.DomainServices.Interfaces;
using System.Data.Entity;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using FluentValidation;
using System.Configuration;

namespace LibrariesBookInventory
{
    public class Global : HttpApplication, IContainerProviderAccessor
    {
        private static IContainerProvider _containerProvider;
        public IContainerProvider ContainerProvider => _containerProvider;


        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            var configuration = MediatRConfigurationBuilder
            .Create(LibrariesBookInventoryApplicationAssemblyHelper.GetLogicAssembly())
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Scoped)
            .Build();

            builder.RegisterMediatR(configuration);


            builder
                .RegisterType<ApplicationDbContext>()
                .As<DbContext>()
                .WithParameter("connectionString", ConfigurationManager.AppSettings["connectionString"]);
            
            MapperConfiguration mapperConfiguration =
                new MapperConfiguration(cfg => cfg.AddMaps(LibrariesBookInventoryApplicationAssemblyHelper.GetLogicAssembly()));

            builder.RegisterInstance(mapperConfiguration.CreateMapper())
                .As<IMapper>()
                .SingleInstance();


            builder.RegisterType<BookRepository>().As<IBookRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
               .As(typeof(IPipelineBehavior<,>))
               .InstancePerDependency();

            builder.RegisterAssemblyTypes(LibrariesBookInventoryApplicationAssemblyHelper.GetLogicAssembly())
               .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
               .AsImplementedInterfaces()
               .InstancePerRequest();

            _containerProvider = new ContainerProvider(builder.Build());
        }
    }
}