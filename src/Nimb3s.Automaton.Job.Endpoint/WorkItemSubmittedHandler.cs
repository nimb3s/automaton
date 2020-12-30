using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class WorkItemSubmittedHandler : IHandleMessages<UserSubmittedWorkItemMessage>
    {
        static ILog log = LogManager.GetLogger<JobSumittedHandler>();

        #region MessageHandler
        public async Task Handle(UserSubmittedWorkItemMessage message, IMessageHandlerContext context)
        {
            WorkItemRepository workItemRepository = new WorkItemRepository();

            await workItemRepository.AddAsync(new Core.Entities.WorkItemEntity
            {
                Id = message.WorkItemId,
                JobId = message.AutomationJobId,
                WorkItemStatusId = (short)message.WorkItemStatus
            });


            foreach (var item in message.HttpRequests)
            {
                await context.Send(new UserSubmittedHttpRequestMessage
                {
                    HttpRequest = item
                }).ConfigureAwait(false);
            }

            log.Info($"MESSAGE: {nameof(UserSubmittedWorkItemMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
