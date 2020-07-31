using Autofac;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.FeedTemps;
using ReadersHub.Business.Service.Store;
using ReadersHub.Model;
using Repository.Pattern.DataContext;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace ReadersHub.NotificationService.AppConfig
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // add registrations here...
            builder.RegisterType<UnitOfWork>().InstancePerLifetimeScope().As<IUnitOfWork>();
            builder.RegisterType<ReadersHubContext>().InstancePerLifetimeScope().As<IDataContext>();
            builder.RegisterType<Repository<Criterion>>().InstancePerLifetimeScope().As<IRepository<Criterion>>();
            builder.RegisterType<CriterionService>().InstancePerLifetimeScope().As<ICriterionService>();
            builder.RegisterType<Repository<FeedTemp>>().InstancePerLifetimeScope().As<IRepository<FeedTemp>>();
            builder.RegisterType<FeedTempService>().InstancePerLifetimeScope().As<IFeedTempService>();
            builder.RegisterType<Repository<Store>>().InstancePerLifetimeScope().As<IRepository<Store>>();
            builder.RegisterType<StoreService>().InstancePerLifetimeScope().As<IStoreService>();

            return builder.Build();
        }
    }
}
