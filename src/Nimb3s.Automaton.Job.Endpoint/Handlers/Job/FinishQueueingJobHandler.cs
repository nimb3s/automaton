using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class FinishQueueingJobHandler :IHandleMessages<FinishedQueueingJobMessage>
    {
        static ILog log = LogManager.GetLogger<FinishQueueingJobHandler>();

        #region MessageHandler
        public async Task Handle(FinishedQueueingJobMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.JobStatusRepository.UpsertAsync(new JobStatusEntity
            {
                JobId = message.JobId,
                JobStatusId = (short)JobStatus.Completed,
                StatusTimeStamp = DateTimeOffset.UtcNow
            });

            dbContext.Commit();

            log.Info($"MESSAGE: {nameof(FinishedQueueingJobMessage)}; HANDLED BY: {nameof(FinishQueueingJobHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
