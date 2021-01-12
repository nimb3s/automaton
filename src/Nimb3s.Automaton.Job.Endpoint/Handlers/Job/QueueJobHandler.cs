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
    public class QueueJobHandler :IHandleMessages<QueueJobMessage>
    {
        static ILog log = LogManager.GetLogger<QueueJobHandler>();

        #region MessageHandler
        public async Task Handle(QueueJobMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.JobRepository.UpsertAsync(new JobEntity
            {
                Id = message.JobId,
                JobName = message.Name,
                InsertTimeStamp = message.CreateDate
            });

            await dbContext.JobStatusRepository.UpsertAsync(new JobStatusEntity
            {
                JobId = message.JobId,
                JobStatusId = (short)JobStatus.Completed,
                StatusTimeStamp = DateTimeOffset.UtcNow
            });

            dbContext.Commit();

            log.Info($"MESSAGE: {nameof(QueueJobMessage)}; HANDLED BY: {nameof(QueueJobHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
