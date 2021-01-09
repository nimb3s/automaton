using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class ExecuteHttpRequestHandler: IHandleMessages<ExecuteHttpRequestMessage>
    {
        static ILog log = LogManager.GetLogger<ExecuteHttpRequestHandler>();

        #region MessageHandler
        public async Task Handle(ExecuteHttpRequestMessage message, IMessageHandlerContext context)
        {
            await SaveRequestAsync(message);
            await SaveResponseAsync(message);

            await context.SendLocal(new HttpRequestExecutedMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequest = message.HttpRequest
            }).ConfigureAwait(false);

            log.Info($"MESSAGE: {nameof(ExecuteHttpRequestMessage)}; HANDLED BY: {nameof(ExecuteHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");
        }

        private async Task SaveRequestAsync(ExecuteHttpRequestMessage message)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.HttpRequestRepository.UpsertAsync(new HttpRequestEntity
            {
                Id = message.HttpRequest.HttpRequestId,
                WorkItemId = message.WorkItemId,
                Url = message.HttpRequest.Url,
                ContentType = message.HttpRequest.ContentType,
                Method = message.HttpRequest.Method,
                Content = message.HttpRequest.Content,
                RequestHeadersInJson = message.HttpRequest.RequestHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.RequestHeaders),
                ContentHeadersInJson = message.HttpRequest.ContentHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.ContentHeaders),
                AuthenticationConfigInJson = message.HttpRequest.AuthenticationConfig == null ? null : JsonConvert.SerializeObject(message.HttpRequest.AuthenticationConfig),
                HttpRequestStatusId = (short)message.HttpRequest.HttpRequestStatus
            });

            dbContext.Commit();
        }

        private async Task SaveResponseAsync(ExecuteHttpRequestMessage message)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            //TODO: Finish the request
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(message.HttpRequest.Url),
            };
            HttpResponseMessage responseMessage = new HttpResponseMessage();

            switch (message.HttpRequest.Method.ToLower())
            {
                case "get":
                    requestMessage.Method = HttpMethod.Get;
                    break;
                case "post":
                    requestMessage.Method = HttpMethod.Post;
                    break;
                default:
                    break;
            }

            responseMessage = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            await dbContext.HttpResponseRepository.UpsertAsync(new HttpResponseEntity
            {
                HttpRequestId = message.HttpRequest.HttpRequestId,
                StatusCode = (int)responseMessage.StatusCode,
                Body = await responseMessage.Content?.ReadAsStringAsync(),
                //RequestHeaders = responseMessage.Headers,
                //ContentHeaders = responseMessage?.Content?.Headers,
            });

            dbContext.Commit();
        }
        #endregion
    }
}
