using System;
using System.Threading.Tasks;
using eventPublisher.data.entities;
using Microsoft.EntityFrameworkCore;

namespace eventPublisher.data
{
    public interface IContext : IDisposable
    {
        DbSet<ApplicationEntity> Applications { get; }
        DbSet<ApplicationEventEntity> ApplicationEvents { get; }
        DbSet<TopicEntity> Topics { get; }
        DbSet<SubscriptionEntity> Subscriptions { get; set; }
        string ProviderName { get; }

        void Migrate();

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}