using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration.UserEntities
{
    public class UserDataConfiguration : BaseAuditableEntityConfiguration<UserData, int?>
    {
        public override void Configure(EntityTypeBuilder<UserData> builder)
        {
            base.Configure(builder);

            builder.ToTable("UsersData");

            builder.Property(u => u.DNI).IsRequired();
            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.Category);

            builder.Property(u => u.BirthDate);

            builder.HasMany(u => u.FamilyGroupPersons)
                .WithOne()
                .HasForeignKey(u => u.GrupoFamiliarId)
                ;

            builder.HasOne<UserAuth>()
                .WithOne()
                .HasForeignKey<UserData>(u => u.UserAuthId)
                .IsRequired();

        }
    }
}
