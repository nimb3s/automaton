using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages;
using Nimb3s.Automaton.Messages.Jobs;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class JobSumittedHandler //:IHandleMessages<UserQueueingJobMessage>
    {
        static ILog log = LogManager.GetLogger<JobSumittedHandler>();

        #region MessageHandler
        public async Task Handle(UserQueueingJobMessage message, IMessageHandlerContext context)
        {
            JobRepository repo = new JobRepository();

            await repo.AddAsync(new JobEntity
            {
                Id = message.JobId,
                JobStatusId = (short)message.JobStatus,
                JobName = message.JobName
            });

            log.Info($"MESSAGE: {nameof(UserQueueingJobMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
