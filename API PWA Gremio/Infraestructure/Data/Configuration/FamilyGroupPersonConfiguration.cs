using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration
{
    public class FamilyGroupPersonConfiguration : BaseAuditableEntityConfiguration<PersonaGrupoFamiliar, int?>
    {
        public override void Configure(EntityTypeBuilder<PersonaGrupoFamiliar> builder)
        {
            base.Configure(builder);

            builder.ToTable("FamilyGroupPersons");
            builder.Property(u => u.Name).IsRequired().HasMaxLength(128);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(128);
            builder.Property(u => u.IsBeneficiaryOfMedical).IsRequired().HasMaxLength(128);
            builder.Property(u => u.IsAffiliate).IsRequired().HasMaxLength(128);

            builder.HasOne<UserData>()
                .WithMany(e => e.FamilyGroupPersons)
                .HasForeignKey(e => e.GrupoFamiliarId)
                .IsRequired();

            

        }
    }
}
