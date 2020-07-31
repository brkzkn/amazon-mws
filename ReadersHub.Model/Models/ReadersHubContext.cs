using Repository.Pattern.DataContext;

namespace ReadersHub.Model
{
    public class ReadersHubContext : DataContext
    {
        public System.Data.Entity.DbSet<Criterion> Criteria { get; set; } // Criterion
        public System.Data.Entity.DbSet<FeedTemp> FeedTemps { get; set; } // Feed_Temp
        public System.Data.Entity.DbSet<Product> Products { get; set; } // Product
        public System.Data.Entity.DbSet<Store> Stores { get; set; } // Store
        public System.Data.Entity.DbSet<User> Users { get; set; } // Users
        public System.Data.Entity.DbSet<UserRole> UserRoles { get; set; } // User_Role
        static ReadersHubContext()
        {
            System.Data.Entity.Database.SetInitializer<ReadersHubContext>(null);
        }

        public ReadersHubContext()
            : base("Name=ReadersHub")
        {
        }

        public ReadersHubContext(string connectionString)
            : base(connectionString)
        {
        }

        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CriterionMap());
            modelBuilder.Configurations.Add(new FeedTempMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new StoreMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
        }
		
    }
}

