using Newtonsoft.Json;
using Nimb3s.Automaton.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class WorkItemSubmittedHandler : IHandleMessages<UserSubmittedWorkItemMessage>
    {
        static ILog log = LogManager.GetLogger<JobSumittedHandler>();

        #region MessageHandler
        public Task Handle(UserSubmittedWorkItemMessage message, IMessageHandlerContext context)
        {
            log.Info($"MESSAGE: {nameof(UserSubmittedWorkItemMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
            return Task.CompletedTask;
        }
        #endregion
    }
}
