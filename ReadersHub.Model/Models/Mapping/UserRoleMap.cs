
namespace ReadersHub.Model
{

    // User_Role
    
    public class UserRoleMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
            : this("dbo")
        {
        }

        public UserRoleMap(string schema)
        {
            ToTable(schema + ".User_Role");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName(@"User_Id").IsRequired().HasColumnType("int");
            Property(x => x.RoleName).HasColumnName(@"Role_Name").IsRequired().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);

            // Foreign keys
            HasRequired(a => a.User).WithMany(b => b.UserRoles).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_User_Role_Users
        }
    }

}

