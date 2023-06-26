using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using FluentValidation;
using LibrariesBookInventory.Application;
using LibrariesBookInventory.DomainServices.Implementation;
using LibrariesBookInventory.DomainServices.Interfaces;
using LibrariesBookInventory.Persistence;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace LibrariesBookInventoryApi
{
    public static class WebApiConfig
    {
        public static IContainer DependencyContainer { get; private set; }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ContainerBuilder();
            DependencyRegistrationAction(builder);
            DependencyContainer = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(DependencyContainer);
        }

        public static Action<ContainerBuilder> DependencyRegistrationAction = builder =>
        {
            var config = GlobalConfiguration.Configuration;

            var configuration = MediatRConfigurationBuilder
                .Create(LibrariesBookInventoryApplicationAssemblyHelper.GetLogicAssembly())
                .WithAllOpenGenericHandlerTypesRegistered()
                .WithRegistrationScope(RegistrationScope.Scoped)
                .Build();

            builder.RegisterMediatR(configuration);

            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerDependency();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(LibrariesBookInventoryApplicationAssemblyHelper.GetLogicAssembly())
                   .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            MapperConfiguration mapperConfiguration =
                new MapperConfiguration(cfg => cfg.AddMaps(LibrariesBookInventoryApplicationAssemblyHelper.GetLogicAssembly()));

            builder.RegisterInstance(mapperConfiguration.CreateMapper())
                .As<IMapper>()
                .SingleInstance();

            builder
                    .RegisterType<ApplicationDbContext>().As<DbContext>()
                    .WithParameter("connectionString", ConfigurationManager.AppSettings["connectionString"])
                    .InstancePerLifetimeScope();
            builder.RegisterType<BookRepository>().As<IBookRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
        };
    }
}
