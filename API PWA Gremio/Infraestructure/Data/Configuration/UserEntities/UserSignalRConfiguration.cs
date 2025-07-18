using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration.UserEntities
{
    public class UserSignalRConfiguration : IEntityTypeConfiguration<UserSignalR>
    {
        public void Configure(EntityTypeBuilder<UserSignalR> builder)
        {
            builder.ToTable("UsersSignalR");
            builder.Property(u => u.ConectionsIdOfUser);

            builder.Property(u => u.SentAnnoucementsById);



            builder.HasOne<UserAuth>()
            .WithOne()
            .HasForeignKey<UserSignalR>(u => u.UserAuthId)
            .IsRequired();

            builder.HasMany(u => u.GroupsOfUser)
                .WithOne(g => g.UserSignalR)
                .HasForeignKey(u => u.UserSignalRId)
                .IsRequired();

        }
    }
}
