using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Entities.Groups;
using System.Reflection.Emit;

namespace PWA_GREMIO_API.Infraestructure.Data.Configuration.Groups
{
    public class GroupConfiguration : BaseAuditableEntityConfiguration<Group, int?>
    {
        public override void Configure(EntityTypeBuilder<Group> builder)
        {
            base.Configure(builder);

            builder.ToTable("SignalRGroups");


            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.Description).IsRequired();

          
         


        }
    }
}
