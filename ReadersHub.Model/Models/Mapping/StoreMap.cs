
namespace ReadersHub.Model
{

    // Store
    
    public class StoreMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Store>
    {
        public StoreMap()
            : this("dbo")
        {
        }

        public StoreMap(string schema)
        {
            ToTable(schema + ".Store");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.SellerId).HasColumnName(@"SellerId").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.MarketPlaceId).HasColumnName(@"MarketPlaceId").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.CurrencyCode).HasColumnName(@"CurrencyCode").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(3);
            Property(x => x.Name).HasColumnName(@"Name").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
        }
    }

}

