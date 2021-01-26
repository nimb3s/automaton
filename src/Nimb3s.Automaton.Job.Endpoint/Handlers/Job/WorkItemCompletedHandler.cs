using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories.Sql;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Pocos;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class WorkItemCompletedHandler : IHandleMessages<WorkItemCompletedMessage>
    {
        static ILog log = LogManager.GetLogger<UserFinishedQueueingJobHandler>();

        #region MessageHandler
        public async Task Handle(WorkItemCompletedMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.WorkItemStatusRepository.UpsertAsync(new WorkItemStatusEntity
            {
                StatusTimeStamp = message.DateActionTaken,
                WorkItemId = message.WorkItemId,
                WorkItemStatusTypeId = (short)WorkItemStatusType.Completed
            });

            dbContext.Commit();

            log.Info($"MESSAGE: {nameof(WorkItemCompletedMessage)}; HANDLED BY: {nameof(WorkItemCompletedHandler)}; JID:{message.JobId}; WID:{message.WorkItemId}");
        }
        #endregion
    }
}
