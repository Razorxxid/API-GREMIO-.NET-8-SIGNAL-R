using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PWA_GREMIO_API.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration
{
    public class AnnoucementConfiguration : BaseAuditableEntityConfiguration<AnnoucementEntity, int?>
    {
        public override void Configure(EntityTypeBuilder<AnnoucementEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("Announcements");
            builder.HasKey(x => x.Id);

            builder.Property(u => u.Id).ValueGeneratedOnAdd();


            builder.Property(u => u.Title).IsRequired();
            builder.Property(u => u.Text).IsRequired();
            builder.Property(u => u.Image_url).IsRequired();
            builder.Property(u => u.DateOfExpiration).IsRequired();
            builder.Property(u => u.TimeOfExpiration).IsRequired();

            builder.Property(u => u.DestinationGroupsIds)
                .HasMaxLength(1024)
                .IsUnicode(false);


            builder.HasOne(u => u.AuthorUserSignalR)
                .WithMany()
                .HasForeignKey(u => u.AuthorUserSignalRId);

            builder.HasMany<AnnoucementOfGroup>()
                .WithOne(u => u.Annoucement)
                .HasForeignKey(u => u.AnnoucementId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
