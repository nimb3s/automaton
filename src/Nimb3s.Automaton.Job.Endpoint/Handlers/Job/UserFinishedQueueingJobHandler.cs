﻿using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories.Sql;
using Nimb3s.Automaton.Messages.User;
using Nimb3s.Automaton.Pocos;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class UserFinishedQueueingJobHandler :IHandleMessages<UserFinishedQueueingJobMessage>
    {
        static ILog log = LogManager.GetLogger<UserFinishedQueueingJobHandler>();

        #region MessageHandler
        public async Task Handle(UserFinishedQueueingJobMessage message, IMessageHandlerContext context)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.JobStatusRepository.UpsertAsync(new JobStatusEntity
            {
                JobId = message.JobId,
                JobStatusTypeId = (short)JobStatusType.FinishedQueueing,
                StatusTimeStamp = message.ActionTookPlaceDate,
            });

            dbContext.Commit();

            log.Info($"MESSAGE: {nameof(UserFinishedQueueingJobMessage)}; HANDLED BY: {nameof(UserFinishedQueueingJobHandler)}; JID:{message.JobId}");
        }
        #endregion
    }
}
