using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class JobSaga : 
        Saga<JobSaga.JobSagaData>,

        IAmStartedByMessages<UserQueueingJobMessage>,
        IAmStartedByMessages<UserFinishedQueueingJobMessage>,
        IAmStartedByMessages<UserSubmittedWorkItemMessage>,
        

        IHandleMessages<UserQueueingJobMessage>,
        IHandleMessages<UserFinishedQueueingJobMessage>,
        IHandleMessages<UserSubmittedWorkItemMessage>
    {
        public class JobSagaData : ContainSagaData
        {
            public Guid JobId { get; set; }
            public int WorkItemCounter { get; set; }
            public bool IsFinishedQueueing { get; set; }
            public bool IsQueueing { get; set; }
            public bool IsJobStarted { get; set; }
            public bool IsJobCompleted { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<JobSagaData> mapper)
        {
            mapper.ConfigureMapping<UserQueueingJobMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
            mapper.ConfigureMapping<UserFinishedQueueingJobMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
            mapper.ConfigureMapping<UserSubmittedWorkItemMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
        }

        public async Task Handle(UserQueueingJobMessage message, IMessageHandlerContext context)
        {
            Data.IsQueueing = true;

            await context.SendLocal(new QueueJobMessage
            {
                JobId = message.JobId,
                Status = message.JobStatus,
                Name = message.JobName
            }).ConfigureAwait(false);
        }

        public async Task Handle(UserFinishedQueueingJobMessage message, IMessageHandlerContext context)
        {
            if (message.JobStatus == JobStatus.FinishedQueueing)
            {
                Data.IsFinishedQueueing = true;
                Data.IsQueueing = false;
            }

            await context.SendLocal(new FinishedQueueingJobMessage
            {
                JobId = message.JobId,
                JobStatus = message.JobStatus
            }).ConfigureAwait(false);
        }

        public async Task Handle(UserSubmittedWorkItemMessage message, IMessageHandlerContext context)
        {
            await context.SendLocal(new CreateWorkItemMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                WorkItemStatus = message.WorkItemStatus,
                HttpRequests = message.HttpRequests
            }).ConfigureAwait(false);
        }
    }
}
