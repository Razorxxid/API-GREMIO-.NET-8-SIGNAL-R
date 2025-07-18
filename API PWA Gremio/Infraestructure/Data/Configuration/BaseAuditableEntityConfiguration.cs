using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration
{
    public abstract class BaseAuditableEntityConfiguration<T, TI> : IEntityTypeConfiguration<T> where T : Auditable<TI>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.CreatedDate).IsRequired();
            builder.Property(t => t.CreatedBy).IsRequired();
            builder.Property(t => t.DeletedBy).HasMaxLength(256);
            builder.Property(t => t.DeletedDate);
            builder.Property(t => t.ModifiedBy).HasMaxLength(256);
        }
    }
}