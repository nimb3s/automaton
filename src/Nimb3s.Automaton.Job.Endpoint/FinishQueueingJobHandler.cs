using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class FinishQueueingJobHandler :IHandleMessages<FinishedQueueingJobMessage>
    {
        static ILog log = LogManager.GetLogger<QueueJobHandler>();

        #region MessageHandler
        public async Task Handle(FinishedQueueingJobMessage message, IMessageHandlerContext context)
        {
            JobRepository repo = new JobRepository();

            await repo.UpdateAsync(new JobEntity
            {
                Id = message.JobId,
                JobStatusId = (short)message.JobStatus
            });

            log.Info($"MESSAGE: {nameof(FinishedQueueingJobMessage)}; HANDLED BY: {nameof(QueueJobHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
