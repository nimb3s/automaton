using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Nimb3s.Automaton.Messages;
using Nimb3s.Automaton.Api.Models;
using Newtonsoft.Json;
using Nimb3s.Automaton.Messages.Jobs;

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
                    var endpointConfiguration = new EndpointConfiguration(typeof(AutomationJobModel).Assembly.GetName().Name);
                    var transport = endpointConfiguration.UseTransport<LearningTransport>();
                    transport.Routing().RouteToEndpoint(
                        assembly: typeof(UserSubmittedAutomationJobMessage).Assembly,
                        @namespace: typeof(UserSubmittedAutomationJobMessage).Namespace,
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
