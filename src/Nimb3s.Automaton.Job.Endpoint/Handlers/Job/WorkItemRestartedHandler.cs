﻿using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
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
                WorkItemStatusId = (short)WorkItemStatusType.ReStarted,
                StatusTimeStamp = message.DateActionTaken
            });

            log.Info($"MESSAGE: {nameof(WorkItemRestartedMessage)}; HANDLED BY: {nameof(CreateWorkItemHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}