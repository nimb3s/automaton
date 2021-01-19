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
    public class UserCreatedJobHandler :IHandleMessages<UserCreatedJobMessage>
    {
        static ILog log = LogManager.GetLogger<UserCreatedJobHandler>();

        #region MessageHandler
        public async Task Handle(UserCreatedJobMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.JobRepository.UpsertAsync(new JobEntity
            {
                Id = message.JobId,
                JobName = message.JobName,
                InsertTimeStamp = message.DateActionTookPlace
            });

            await dbContext.JobStatusRepository.UpsertAsync(new JobStatusEntity
            {
                JobId = message.JobId,
                JobStatusTypeId = (short)JobStatusType.Created,
                StatusTimeStamp = DateTimeOffset.UtcNow
            });

            dbContext.Commit();

            log.Info($"MESSAGE: {nameof(UserCreatedJobMessage)}; HANDLED BY: {nameof(UserCreatedJobHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
