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
    public class RestartWorkItemHandler : IHandleMessages<WorkItemRestartedMessage>
    {
        static ILog log = LogManager.GetLogger<CreateWorkItemHandler>();

        #region MessageHandler
        public async Task Handle(WorkItemRestartedMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.WorkItemStatusRepository.UpsertAsync(new WorkItemStatusEntity
            {
                WorkItemId = message.WorkItemId,
                WorkItemStatusTypeId = (short)WorkItemStatusType.ReStarted,
                StatusTimeStamp = message.DateActionTaken
            });

            dbContext.Commit();

            log.Info($"MESSAGE: {nameof(WorkItemRestartedMessage)}; HANDLED BY: {nameof(CreateWorkItemHandler)}; JID:{message.JobId}; WID:{message.WorkItemId}");
        }
        #endregion
    }
}
