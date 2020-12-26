using System;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NServiceBus;

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
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                //.Settings(new JsonSerializerSettings
                //{
                //    TypeNameHandling = TypeNameHandling.Auto,
                //    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                //});

            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
