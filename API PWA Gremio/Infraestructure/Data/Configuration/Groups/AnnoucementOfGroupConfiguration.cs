using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration.Groups
{
    public class AnnoucementOfGroupConfiguration : IEntityTypeConfiguration<AnnoucementOfGroup>
    {
        public void Configure(EntityTypeBuilder<AnnoucementOfGroup> builder)
        {
            builder.ToTable("AnnoucementsOfGroups");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd(); 


            builder.HasOne(u => u.Annoucement)
                .WithMany()
                .HasForeignKey(u => u.AnnoucementId)
                .OnDelete(DeleteBehavior.Cascade); // Opción para eliminar los mensajes asociados al eliminar un grupo

            builder.HasOne(u => u.UserSignalR)
                .WithMany(u => u.AnnoucementsOfUser)
                .HasForeignKey(u => u.UserSignalRId)
                .OnDelete(DeleteBehavior.ClientSetNull); // Opción para establecer NULL en la clave externa

            builder.HasOne(u => u.Group)
                .WithMany(g => g.AnnoucementsOfGroup)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.Restrict); // Evitar que se elimine un grupo si hay mensajes asociados

        }
    }
}
