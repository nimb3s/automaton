using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Nimb3s.Automaton.Constants;
using Nimb3s.Automaton.Messages.User;
using Nimb3s.Automaton.Pocos.Models;
using NServiceBus;
using System.Diagnostics;
using System.IO;

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
                    var endpointConfiguration = new EndpointConfiguration(typeof(JobCreatedModel).Assembly.GetName().Name);
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.StorageDirectory(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
                    // transport.ConnectionString(@"Data Source=.;Initial Catalog=Automaton;Integrated Security=true");
                    transport.Routing().RouteToEndpoint(
                        assembly: typeof(UserCreatedJobMessage).Assembly,
                        @namespace: typeof(UserCreatedJobMessage).Namespace,
                        destination: AutomatonConstants.MessageBus.JobEndpoint.ENDPOINT_NAME
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
