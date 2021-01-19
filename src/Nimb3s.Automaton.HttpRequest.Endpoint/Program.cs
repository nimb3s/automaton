using System;
using System.Reflection;
using System.Threading.Tasks;
using Nimb3s.Automaton.Messages.HttpRequests;
using NServiceBus;

namespace Nimb3s.Automaton.HttpRequest.Endpoint
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
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
