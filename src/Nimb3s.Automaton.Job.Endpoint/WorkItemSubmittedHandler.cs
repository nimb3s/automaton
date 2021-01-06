using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.HttpRequests;
using Nimb3s.Automaton.Messages.Jobs;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class WorkItemSubmittedHandler //: IHandleMessages<UserSubmittedWorkItemMessage>
    {
        static ILog log = LogManager.GetLogger<JobSumittedHandler>();

        #region MessageHandler
        public async Task Handle(UserSubmittedWorkItemMessage message, IMessageHandlerContext context)
        {
            WorkItemRepository workItemRepository = new WorkItemRepository();

            await workItemRepository.AddAsync(new Core.Entities.WorkItemEntity
            {
                Id = message.WorkItemId,
                JobId = message.JobId,
                WorkItemStatusId = (short)message.WorkItemStatus
            });

            foreach (var item in message.HttpRequests)
            {
                await context.Send(new UserSubmittedHttpRequestMessage
                {
                    WorkItemId = message.WorkItemId,
                    HttpRequest = item
                }).ConfigureAwait(false);
            }

            log.Info($"MESSAGE: {nameof(UserSubmittedWorkItemMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
