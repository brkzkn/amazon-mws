using Autofac;
using Autofac.Integration.Mvc;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.Product;
using ReadersHub.Business.Service.Store;
using ReadersHub.Business.Service.User;
using ReadersHub.Model;
using Repository.Pattern.DataContext;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System.Web.Mvc;

namespace ReadersHub.WebApplication
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // add registrations here...
            builder.RegisterType<UnitOfWork>().InstancePerLifetimeScope().As<IUnitOfWork>();
            builder.RegisterType<ReadersHubContext>().InstancePerLifetimeScope().As<IDataContext>();

            builder.RegisterType<Repository<Criterion>>().InstancePerLifetimeScope().As<IRepository<Criterion>>();
            builder.RegisterType<CriterionService>().InstancePerLifetimeScope().As<ICriterionService>();

            builder.RegisterType<Repository<Store>>().InstancePerLifetimeScope().As<IRepository<Store>>();
            builder.RegisterType<StoreService>().InstancePerLifetimeScope().As<IStoreService>();

            builder.RegisterType<Repository<Product>>().InstancePerLifetimeScope().As<IRepository<Product>>();
            builder.RegisterType<ProductService>().InstancePerLifetimeScope().As<IProductService>();

            builder.RegisterType<Repository<User>>().InstancePerLifetimeScope().As<IRepository<User>>();
            builder.RegisterType<Repository<UserRole>>().InstancePerLifetimeScope().As<IRepository<UserRole>>();
            builder.RegisterType<UserService>().InstancePerLifetimeScope().As<IUserService>();


            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}