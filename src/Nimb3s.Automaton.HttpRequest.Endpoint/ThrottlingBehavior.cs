using Nimb3s.Automaton.Constants;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.HttpRequest.Endpoint
{
    //https://docs.particular.net/samples/throttling/
    public class ThrottlingBehavior :
        Behavior<IInvokeHandlerContext>
    {
        static ILog log = LogManager.GetLogger<ThrottlingBehavior>();
        static DateTime? nextRateLimitReset = DateTime.UtcNow.AddSeconds(AutomatonConstants.MessageBus.HttpRequestEndpoint.RateLimitInSeconds.Value);

        public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
        {
            var rateLimitReset = nextRateLimitReset;

            if (rateLimitReset.HasValue && rateLimitReset >= DateTime.UtcNow)
            {
                var localTime = rateLimitReset?.ToLocalTime();
                log.Info($"Rate limit exceeded. Retry after {rateLimitReset} UTC ({localTime} local).");
                await DelayMessage(context, rateLimitReset.Value)
                    .ConfigureAwait(false);
                return;
            }

            await next()
                .ConfigureAwait(false);

            nextRateLimitReset = DateTime.UtcNow.AddSeconds(30);
        }

        Task DelayMessage(IInvokeHandlerContext context, DateTime deliverAt)
        {
            var sendOptions = new SendOptions();

            // delay the message to the specified delivery date
            sendOptions.DoNotDeliverBefore(deliverAt);
            // send message to this endpoint
            sendOptions.RouteToThisEndpoint();
            // maintain the original ReplyTo address
            if (context.Headers.TryGetValue(Headers.ReplyToAddress, out var replyAddress))
            {
                sendOptions.RouteReplyTo(replyAddress);
            }

            return context.Send(context.MessageBeingHandled, sendOptions);
        }
    }
}
