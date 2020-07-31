
namespace ReadersHub.Model
{

    // Product
    
    public class ProductMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Product>
    {
        public ProductMap()
            : this("dbo")
        {
        }

        public ProductMap(string schema)
        {
            ToTable(schema + ".Product");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Isbn).HasColumnName(@"ISBN").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.IsbnName).HasColumnName(@"ISBN_Name").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(1500);
            Property(x => x.Asin).HasColumnName(@"ASIN").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.AsinName).HasColumnName(@"ASIN_Name").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(1500);
            Property(x => x.PriceUpdateTimeUK).HasColumnName(@"Price_Update_Time_UK").IsOptional().HasColumnType("datetime");
            Property(x => x.PriceUpdateTimeUS).HasColumnName(@"Price_Update_Time_US").IsOptional().HasColumnType("datetime");
            Property(x => x.MinNewIsbnPriceDollar).HasColumnName(@"Min_New_ISBN_Price_Dollar").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinUsedIsbnPriceDollar).HasColumnName(@"Min_Used_ISBN_Price_Dollar").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinNewIsbnPricePound).HasColumnName(@"Min_New_ISBN_Price_Pound").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinUsedIsbnPricePound).HasColumnName(@"Min_Used_ISBN_Price_Pound").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinNewAsinPriceDollar).HasColumnName(@"Min_New_ASIN_Price_Dollar").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinUsedAsinPriceDollar).HasColumnName(@"Min_Used_ASIN_Price_Dollar").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinNewAsinPricePound).HasColumnName(@"Min_New_ASIN_Price_Pound").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.MinUsedAsinPricePound).HasColumnName(@"Min_Used_ASIN_Price_Pound").IsOptional().HasColumnType("smallmoney").HasPrecision(10, 4);
            Property(x => x.IsFixedNewDollar).HasColumnName(@"Is_Fixed_New_Dollar").IsOptional().HasColumnType("bit");
            Property(x => x.IsFixedUsedDollar).HasColumnName(@"Is_Fixed_Used_Dollar").IsOptional().HasColumnType("bit");
            Property(x => x.IsFixedNewPound).HasColumnName(@"Is_Fixed_New_Pound").IsOptional().HasColumnType("bit");
            Property(x => x.IsFixedUsedPound).HasColumnName(@"Is_Fixed_Used_Pound").IsOptional().HasColumnType("bit");

        }
    }

}

