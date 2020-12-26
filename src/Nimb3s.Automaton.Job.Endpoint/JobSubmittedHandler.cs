using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class JobSumittedHandler :IHandleMessages<UserSubmittedAutomationJobMessage>
    {
        static ILog log = LogManager.GetLogger<JobSumittedHandler>();

        #region MessageHandler
        public async Task Handle(UserSubmittedAutomationJobMessage message, IMessageHandlerContext context)
        {
            JobRepository repo = new JobRepository();

            await repo.AddAsync(new JobEntity
            {
                Id = message.AutomationJobId,
                JobStatusId = 0,//message.AutomationJobStatus,
                JobName = message.AutomationJobName
            });

            log.Info($"MESSAGE: {nameof(UserSubmittedAutomationJobMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
