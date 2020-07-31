using Autofac;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.Product;
using ReadersHub.Model;
using Repository.Pattern.DataContext;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace ReadersHub.PriceUpdateServiceUK.Config
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // add registrations here...
            builder.RegisterType<UnitOfWork>().InstancePerLifetimeScope().As<IUnitOfWork>();
            builder.RegisterType<ReadersHubContext>().InstancePerLifetimeScope().As<IDataContext>();
            builder.RegisterType<ProductService>().InstancePerLifetimeScope().As<IProductService>();
            builder.RegisterType<CriterionService>().InstancePerLifetimeScope().As<ICriterionService>();
            builder.RegisterType<Repository<Product>>().InstancePerLifetimeScope().As<IRepository<Product>>();
            builder.RegisterType<Repository<Criterion>>().InstancePerLifetimeScope().As<IRepository<Criterion>>();
            
            return builder.Build();
        }
    }
}
