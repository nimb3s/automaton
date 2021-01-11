using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
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

            await dbContext.WorkItemRepository.UpsertAsync(new Core.Entities.WorkItemEntity
            {
                Id = message.WorkItemId,
                JobId = message.JobId,
                WorkItemStatusId = (short)message.WorkItemStatus,
                InsertTimeStamp = message.CreateDate
            });

            log.Info($"MESSAGE: {nameof(WorkItemCreatedMessage)}; HANDLED BY: {nameof(CreateWorkItemHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
