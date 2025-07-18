using Microsoft.EntityFrameworkCore;
using PWA_GREMIO_API.Infraestructure.Data.Configuration;
using PWA_GREMIO_API.Infraestructure.Data.Configuration.Groups;
using PWA_GREMIO_API.Infraestructure.Data.Configuration.UserEntities;

namespace PWA_GREMIO_API.Infraestructure.Data
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions options) : base(options)
        {
        }

        static readonly ILoggerFactory _loggerFactory = new LoggerFactory(new[]
        {
            new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserAuthConfiguration());
            modelBuilder.ApplyConfiguration(new UserDataConfiguration());
            modelBuilder.ApplyConfiguration(new UserSignalRConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyGroupPersonConfiguration());
            modelBuilder.ApplyConfiguration(new AnnoucementConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserOfGroupConfiguration());
            modelBuilder.ApplyConfiguration(new AnnoucementOfGroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserOfGroupConfiguration());

            Seeds.SeedData.Seeds(modelBuilder);
            Seeds.SeedData.SeedUsersAndAnnoucementsOfGroup(modelBuilder);
        }

    }
}
