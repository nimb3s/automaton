using System;
using System.Reflection;
using System.Threading.Tasks;
using Nimb3s.Automaton.Constants;
using Nimb3s.Automaton.Messages.Job;
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

            if(AutomatonConstants.MessageBus.HttpRequestEndpoint.MessageProcessingConcurrency != null)
            {
                endpointConfiguration.LimitMessageProcessingConcurrencyTo(AutomatonConstants.MessageBus.HttpRequestEndpoint.MessageProcessingConcurrency.Value);
            }
                
            if(AutomatonConstants.MessageBus.HttpRequestEndpoint.RateLimitInSeconds != null)
            {
                endpointConfiguration.Pipeline.Register(typeof(ThrottlingBehavior), "Throttle web requests");
            }
            

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Routing().RouteToEndpoint(
                messageType: typeof(HttpRequestExecutedMessage),
                destination: AutomatonConstants.MessageBus.JobEndpoint.ENDPOINT_NAME
            );

            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
        }
    }
}
