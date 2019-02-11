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
        string ProviderName { get; }

        void Migrate();

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}