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
    public class JobSumittedHandler :IHandleMessages<UserSubmittedAutomationJobMessage>
    {
        static ILog log = LogManager.GetLogger<JobSumittedHandler>();

        #region MessageHandler
        public Task Handle(UserSubmittedAutomationJobMessage message, IMessageHandlerContext context)
        {
            log.Info($"MESSAGE: {nameof(UserSubmittedAutomationJobMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
            return Task.CompletedTask;
        }
        #endregion
    }
}
