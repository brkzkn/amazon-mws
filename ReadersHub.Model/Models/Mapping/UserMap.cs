
namespace ReadersHub.Model
{

    // Users
    
    public class UserMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserMap()
            : this("dbo")
        {
        }

        public UserMap(string schema)
        {
            ToTable(schema + ".Users");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Username).HasColumnName(@"Username").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.Password).HasColumnName(@"Password").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(255);
            Property(x => x.FullName).HasColumnName(@"Full_Name").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(100);
            Property(x => x.Email).HasColumnName(@"Email").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(150);
            Property(x => x.RegisteredDate).HasColumnName(@"Registered_Date").IsOptional().HasColumnType("datetime");
        }
    }

}

