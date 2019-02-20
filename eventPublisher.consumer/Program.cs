using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Confluent.Kafka;
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
            // var config = new ConsumerConfig
            // {
            //     GroupId = "test-consumer-group",
            //     BootstrapServers = "localhost:9092",
            //     // Note: The AutoOffsetReset property determines the start offset in the event
            //     // there are not yet any committed offsets for the consumer group for the
            //     // topic/partitions of interest. By default, offsets are committed
            //     // automatically, so in this example, consumption will only start from the
            //     // earliest message in the topic 'my-topic' the first time you run the program.
            //     AutoOffsetReset = AutoOffsetReset.Earliest
            // };

            // var topics = serviceProvider.GetService<IRepository>().GetTopics();

            // using (var c = new ConsumerBuilder<string, string>(config).Build())
            // {
            //     if (topics.Any()) c.Subscribe(topics);

            //     CancellationTokenSource cts = new CancellationTokenSource();
            //     Console.CancelKeyPress += (_, e) =>
            //     {
            //         e.Cancel = true; // prevent the process from terminating.
            //         cts.Cancel();
            //     };

            //     try
            //     {
            //         while (true)
            //         {
            //             try
            //             {
            //                 var cr = c.Consume(cts.Token);
            //                 Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
            //             }
            //             catch (ConsumeException e)
            //             {
            //                 Console.WriteLine($"Error occured: {e.Error.Reason}");
            //             }
            //         }
            //     }
            //     catch (OperationCanceledException)
            //     {
            //         // Ensure the consumer leaves the group cleanly and final offsets are committed.
            //         c.Close();
            //     }
            // }
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
