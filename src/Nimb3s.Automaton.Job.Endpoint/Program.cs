using Nimb3s.Automaton.Constants;
using Nimb3s.Automaton.Messages.HttpRequest;
using NServiceBus;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    class Program
    {
        static async Task Main()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            Console.Title = assemblyName;
            var endpointConfiguration = new EndpointConfiguration($"{assemblyName}");
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Routing().RouteToEndpoint(
                assembly: typeof(ExecuteHttpRequestMessage).Assembly,
                @namespace: typeof(ExecuteHttpRequestMessage).Namespace,
                destination: AutomatonConstants.MessageBus.HttpRequestEndpoint.ENDPOINT_NAME
            );

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

           
            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
        }
    }
}
