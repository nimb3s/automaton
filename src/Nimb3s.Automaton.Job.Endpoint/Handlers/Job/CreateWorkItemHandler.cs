using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Pocos;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class CreateWorkItemHandler : IHandleMessages<WorkItemCreatedMessage>
    {
        static ILog log = LogManager.GetLogger<CreateWorkItemHandler>();

        #region MessageHandler
        public async Task Handle(WorkItemCreatedMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.WorkItemRepository.UpsertAsync(new WorkItemEntity
            {
                Id = message.WorkItemId,
                JobId = message.JobId,
                InsertTimeStamp = message.CreateDate
            });

            await dbContext.WorkItemStatusRepository.UpsertAsync(new WorkItemStatusEntity
            {
                WorkItemId = message.WorkItemId,
                WorkItemStatusTypeId = (short)WorkItemStatusType.Queued,
                StatusTimeStamp = message.CreateDate,
            });

            dbContext.Commit();


            log.Info($"MESSAGE: {nameof(WorkItemCreatedMessage)}; HANDLED BY: {nameof(CreateWorkItemHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
