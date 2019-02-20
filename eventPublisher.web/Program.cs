using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.data;
using eventPublisher.domain.contracts;
using eventPublisher.domain.utilities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eventPublisher.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = BuildWebHost(args);
			using (IServiceScope scope = host.Services.CreateScope())
			{
				IServiceProvider services = scope.ServiceProvider;
				var context = ((IContext)services.GetService(typeof(IContext)));
				var publisher = ((IPublishEvents)services.GetService(typeof(IPublishEvents)));
				var consumer = ((IConsumeEvents)services.GetService(typeof(IConsumeEvents)));
				var provider = context.ProviderName;

				// if not an InMemory database, migrate
				if (!provider.Contains("InMemory")) 
				{
					context.Migrate();
					consumer.Consume();
					// publisher.SubscribeToTopics();
				}
			}

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddCommandLine(args)
				.Build();
			return WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(config)
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseStartup<Startup>()
				//.UseApplicationInsights()
				.Build();
		}
    }
}
