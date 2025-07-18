using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration.Groups
{
    public class UserOfGroupConfiguration : IEntityTypeConfiguration<UserOfGroup>
    {
        public void Configure(EntityTypeBuilder<UserOfGroup> builder)
        {
            builder.ToTable("UsersOfGroups");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.HasOne(u => u.UserSignalR)
                .WithMany(u => u.GroupsOfUser)
                .HasForeignKey(u => u.UserSignalRId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Opción para establecer NULL en la clave externa
                ;


            builder.HasOne(u => u.Group)
                .WithMany(g => g.UsersOfGroup)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull) // Opción para establecer NULL en la clave externa

                ;
        }
    }
}
