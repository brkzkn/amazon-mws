
namespace ReadersHub.Model
{

    // Feed_Temp
    
    public class FeedTempMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<FeedTemp>
    {
        public FeedTempMap()
            : this("dbo")
        {
        }

        public FeedTempMap(string schema)
        {
            ToTable(schema + ".Feed_Temp");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("bigint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Sku).HasColumnName(@"SKU").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.Asin).HasColumnName(@"ASIN").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.Condition).HasColumnName(@"Condition").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);
            Property(x => x.Price).HasColumnName(@"Price").IsRequired().HasColumnType("money").HasPrecision(10,4);
            Property(x => x.CreateDate).HasColumnName(@"CreateDate").IsRequired().HasColumnType("datetime");
            Property(x => x.Status).HasColumnName(@"Status").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.SellerId).HasColumnName(@"Seller_Id").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
        }
    }

}

