using Nimb3s.Automaton.Messages.HttpRequests;
using Nimb3s.Automaton.Messages.Jobs;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class JobOrchestrator : 
        Saga<JobOrchestrator.JobOrchestratorData>,

        IAmStartedByMessages<UserSubmittedAutomationJobMessage>,
        IAmStartedByMessages<UserSubmittedWorkItemMessage>,
        IAmStartedByMessages<UserSubmittedHttpRequestMessage>,

        IHandleMessages<UserSubmittedAutomationJobMessage>,
        IHandleMessages<UserSubmittedWorkItemMessage>,
        IHandleMessages<UserSubmittedHttpRequestMessage>
    {
        public class JobOrchestratorData : ContainSagaData
        {
            public Guid JobId { get; set; }
            public string JobName { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<JobOrchestratorData> mapper)
        {
            mapper.ConfigureMapping<UserSubmittedAutomationJobMessage>(message => message.JobId)
                .ToSaga(sagaData => sagaData.JobId);
        }

        public Task Handle(UserSubmittedAutomationJobMessage message, IMessageHandlerContext context)
        {
            Data.JobName = message.JobName
        }

        public Task Handle(UserSubmittedWorkItemMessage message, IMessageHandlerContext context)
        {
            
        }

        public Task Handle(UserSubmittedHttpRequestMessage message, IMessageHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
