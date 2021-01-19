using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Messages.User;
using Nimb3s.Automaton.Pocos;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class UserSubmittedHttpRequestHandler: IHandleMessages<UserSubmittedHttpRequestMessage>
    {
        static ILog log = LogManager.GetLogger<UserSubmittedHttpRequestHandler>();

        #region MessageHandler
        public async Task Handle(UserSubmittedHttpRequestMessage message, IMessageHandlerContext context)
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
                UserAgent = message.HttpRequest.UserAgent,
                RequestHeadersInJson = message.HttpRequest.RequestHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.RequestHeaders),
                ContentHeadersInJson = message.HttpRequest.ContentHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.ContentHeaders),
                AuthenticationConfigInJson = message.HttpRequest.AuthenticationConfig == null ? null : JsonConvert.SerializeObject(message.HttpRequest.AuthenticationConfig),
            });

            await dbContext.HttpRequestStatusRepository.UpsertAsync(new HttpRequestStatusEntity
            {
                HttpRequestId = message.HttpRequest.HttpRequestId,
                HttpRequestStatusTypeId = (short)HttpRequestStatusType.Queued,
                StatusTimeStamp = DateTimeOffset.UtcNow,
            });

            log.Info($"MESSAGE: {nameof(UserSubmittedHttpRequestMessage)}; HANDLED BY: {nameof(UserSubmittedHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
