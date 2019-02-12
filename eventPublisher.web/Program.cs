using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eventPublisher.data;
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
				var provider = context.ProviderName;

				// if not an InMemory database, migrate
				if (!provider.Contains("InMemory")) 
				{
					context.Migrate();
					List<string> topics = context.Topics.Select(t => t.Name).ToList();
					topics.ForEach(topic => {
						var bashResult = $"kafka-topics --create --topic {topic} --zookeeper 127.0.0.1:2181 --partitions 1 --replication-factor 1".Bash();
					});
				}
			}

			var output = "dotnet ../eventPublisher.consumer/bin/debug/netcoreapp2.0/eventPublisher.consumer.dll".Bash();

			// var confluentStart = "confluent start".Bash();
			// var topic = "kafka-topics --create --topic verificationtopic --zookeeper 127.0.0.1:2181 --partitions 1 --replication-factor 1".Bash();
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
