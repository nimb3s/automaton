using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Job.Endpoint.Factories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class ExecuteHttpRequestHandler : IHandleMessages<ExecuteHttpRequestMessage>
    {
        static ILog log = LogManager.GetLogger<ExecuteHttpRequestHandler>();

        public async Task Handle(ExecuteHttpRequestMessage message, IMessageHandlerContext context)
        {
            await SetStatusToStarted(message);
            var httpResponseMessage = await SendHttpRequestAsync(message).ConfigureAwait(false);
            await SaveHttpRequestAsync(message, httpResponseMessage).ConfigureAwait(false);

            await context.SendLocal(new HttpRequestExecutedMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequest = message.HttpRequest,
                DateActionTaken = DateTime.UtcNow
            }).ConfigureAwait(false);

            log.Info($"MESSAGE: {nameof(ExecuteHttpRequestMessage)}; HANDLED BY: {nameof(ExecuteHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");
        }

        private async Task SetStatusToStarted(ExecuteHttpRequestMessage message)
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
            }).ConfigureAwait(false);

            await dbContext.HttpRequestStatusRepository.UpsertAsync(new HttpRequestStatusEntity
            {
                HttpRequestId = message.HttpRequest.HttpRequestId,
                HttpRequestStatusTypeId = (short)HttpRequestStatusType.Started,
                StatusTimeStamp = DateTimeOffset.UtcNow,
            }).ConfigureAwait(false);

            dbContext.Commit();
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(ExecuteHttpRequestMessage message)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                //TODO: finish the factory and test it
                //HttpClient client = new HttpClient();
                //HttpRequestMessage httpRequestMessage = await HttpRequestFactory.CreateRequestAsync(message.HttpRequest);
                //httpResponse = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

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

                httpResponse = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                httpResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                httpResponse.Content = new StringContent(ex.StackTrace);
            }

            return httpResponse;
        }

        private async Task SaveHttpRequestAsync(ExecuteHttpRequestMessage message, HttpResponseMessage httpResponseMessage)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();
            var content = await httpResponseMessage.Content?.ReadAsStringAsync();

            await dbContext.HttpResponseRepository.UpsertAsync(new HttpResponseEntity
            {
                Id = Guid.NewGuid(),
                HttpRequestId = message.HttpRequest.HttpRequestId,
                StatusCode = (int)httpResponseMessage.StatusCode,
                Body = content,
                InsertTimeStamp = message.CreateDate
            }).ConfigureAwait(false);

            await dbContext.HttpRequestStatusRepository.UpsertAsync(new HttpRequestStatusEntity
            {
                HttpRequestId = message.HttpRequest.HttpRequestId,
                HttpRequestStatusTypeId = (short)HttpRequestStatusType.Completed,
                StatusTimeStamp = DateTimeOffset.UtcNow,
            }).ConfigureAwait(false);

            dbContext.Commit();
        }
    }
}
