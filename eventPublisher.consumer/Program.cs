using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using eventPublisher.data;
using eventPublisher.domain.contracts;
using eventPublisher.domain.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eventPublisher.consumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            ServiceProvider serviceProvider = ConfigureServices();
            serviceProvider.GetService<IConsumeEvents>().ReceiveEvents();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IConsumeEvents, EventConsumer>();
            services.AddTransient<IRepository, EventPublisherRepository>();
            services.AddDbContext<IContext, EventPublisherContext>(options => options.UseNpgsql("User ID=admin;Password=admin;Host=localhost;Port=5432;Database=EventPublisher"));

            return services.BuildServiceProvider();
        }
    }
}
