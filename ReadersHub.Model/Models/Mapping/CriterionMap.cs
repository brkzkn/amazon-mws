
namespace ReadersHub.Model
{

    // Criterion
    
    public class CriterionMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Criterion>
    {
        public CriterionMap()
            : this("dbo")
        {
        }

        public CriterionMap(string schema)
        {
            ToTable(schema + ".Criterion");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.StoreId).HasColumnName(@"Store_Id").IsRequired().HasColumnType("int");
            Property(x => x.Key).HasColumnName(@"Key").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Value).HasColumnName(@"Value").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(5000);

            // Foreign keys
            HasRequired(a => a.Store).WithMany(b => b.Criteria).HasForeignKey(c => c.StoreId).WillCascadeOnDelete(false); // FK_Criterion_Store
        }
    }

}

