using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration.UserEntities
{
    public class UserAuthConfiguration : IEntityTypeConfiguration<UserAuth>
    {
        public void Configure(EntityTypeBuilder<UserAuth> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.ToTable("UsersAuth");
            builder.Property(u => u.Password).IsRequired().HasMaxLength(256);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.Roles)
                .HasMaxLength(1024)
                .IsUnicode(false);




        }
    }

}
