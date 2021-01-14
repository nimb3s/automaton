using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class UserRestartedJobHandler :IHandleMessages<UserRestartedJobMessage>
    {
        static ILog log = LogManager.GetLogger<UserCreatedJobHandler>();

        #region MessageHandler
        public async Task Handle(UserRestartedJobMessage message, IMessageHandlerContext context)
        {
            await RestartJobAsync(message).ConfigureAwait(false);
            await RestartWorkItemsAsync(message, context).ConfigureAwait(false);

            log.Info($"MESSAGE: {nameof(UserRestartedJobMessage)}; HANDLED BY: {nameof(UserCreatedJobHandler)}: {JsonConvert.SerializeObject(message)}");
        }

        private async Task RestartJobAsync(UserRestartedJobMessage message)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            var job = await dbContext.JobRepository.GetAsync(message.JobId).ConfigureAwait(false);

            await dbContext.JobStatusRepository.UpsertAsync(new JobStatusEntity
            {
                JobId = message.JobId,
                JobStatusTypeId = (short)JobStatusType.Restart,
                StatusTimeStamp = message.DateActionTookPlace
            }).ConfigureAwait(false);

            dbContext.Commit();
        }

        private async Task RestartWorkItemsAsync(UserRestartedJobMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            var httpRequests = await dbContext.HttpRequestRepository.GetAllByJobIdAndStatusAsync(message.JobId, (short)WorkItemStatusType.Completed).ConfigureAwait(false);

            dbContext.Commit();

            var workItems = httpRequests.GroupBy(i => i.WorkItemId).Select(i => new
            {
                WorkItemId = i.Key,
                HttpRequests = i.ToList()
            });

            foreach (var workItem in workItems)
            {
                var restartMessage = new UserRestartedWorkItemMessage();

                restartMessage.JobId = message.JobId;
                restartMessage.WorkItemId = workItem.WorkItemId;
                restartMessage.DateActionTaken = message.DateActionTookPlace;

                List<UserHttpRequest> userHttpRequests = new List<UserHttpRequest>();

                foreach (var httpRequest in workItem.HttpRequests)
                {
                    var request = new UserHttpRequest();

                    if(httpRequest.AuthenticationConfigInJson != null)
                    {
                        request.AuthenticationConfig = JsonConvert.DeserializeObject<HttpAuthenticationConfig>(httpRequest.AuthenticationConfigInJson);
                    }

                    if(httpRequest.ContentHeadersInJson != null)
                    {
                        request.ContentHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpRequest.ContentHeadersInJson);
                    }
                    
                    request.ContentType = httpRequest.ContentType;
                    request.HttpRequestId = httpRequest.Id;
                    request.Method = httpRequest.Method;

                    if(httpRequest.RequestHeadersInJson != null)
                    {
                        request.RequestHeaders = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpRequest.RequestHeadersInJson);
                    }

                    request.Url = httpRequest.Url;
                    request.Content = httpRequest.Content;

                    userHttpRequests.Add(request);
                }

                restartMessage.HttpRequests = userHttpRequests;

                await context.SendLocal(restartMessage)
                             .ConfigureAwait(false);
            }
        }

        #endregion
    }
}
