﻿using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using NServiceBus;
using NServiceBus.Logging;
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
                JobStatusId = (short)message.Status,
                JobName = message.Name
            });

            log.Info($"MESSAGE: {nameof(QueueJobMessage)}; HANDLED BY: {nameof(QueueJobHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}