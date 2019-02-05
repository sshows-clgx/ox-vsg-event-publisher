using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eventPublisher.data.entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace eventPublisher.data
{
    public class EventPublisherContext : DbContext
    {
        public EventPublisherContext() : this(new DbContextOptionsBuilder<EventPublisherContext>().UseNpgsql("User ID=admin;Password=admin;Host=localhost;Port=5432;Database=EventPublisher").Options)
        {
        }

        public EventPublisherContext(DbContextOptions<EventPublisherContext> options) : base(options)
        {
        }

        public DbSet<ApplicationEntity> Applications { get; set; }
        public DbSet<ApplicationEventEntity> ApplicationEvents { get; set; }

        public string ProviderName => base.Database.ProviderName;

        public void Migrate()
        {
            this.Database.Migrate();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationEntity>()
                .Property(b => b.InsertedUtc)
                .HasDefaultValueSql("now() at time zone 'utc'");

            modelBuilder.Entity<ApplicationEventEntity>()
                .Property(b => b.InsertedUtc)
                .HasDefaultValueSql("now() at time zone 'utc'");

            // Postgres is lame. We have to put _ between our table/column names to allow readablitiy (to not be more lame)
            base.OnModelCreating(modelBuilder);
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = ToSnakeCase(entity.Relational().TableName);

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = ToSnakeCase(property.Name);
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = ToSnakeCase(index.Relational().Name);
                }
            }
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}