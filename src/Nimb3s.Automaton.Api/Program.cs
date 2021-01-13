using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nimb3s.Automaton.Api.Models;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;

namespace Nimb3s.Automaton.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration(typeof(NewJobModel).Assembly.GetName().Name);
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.Routing().RouteToEndpoint(
                        assembly: typeof(UserCreatedJobMessage).Assembly,
                        //@namespace: typeof(UserQueueingJobMessage).Namespace,
                        destination: "Nimb3s.Automaton.Job.Endpoint"
                    );

                    endpointConfiguration.SendOnly();
                    endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                        //.Settings(new JsonSerializerSettings
                        //{
                        //    TypeNameHandling = TypeNameHandling.Auto,
                        //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                        //});

                    return endpointConfiguration;
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
