using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages.HttpRequests;
using Nimb3s.Automaton.Messages.Jobs;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class JobOrchestrator : 
        Saga<JobOrchestrator.JobOrchestratorData>,

        IAmStartedByMessages<UserQueueingJobMessage>,
        IAmStartedByMessages<UserFinishedQueueingJobMessage>,
        IAmStartedByMessages<UserSubmittedWorkItemMessage>,
        IAmStartedByMessages<UserSubmittedHttpRequestMessage>,
        

        IHandleMessages<UserQueueingJobMessage>,
        IHandleMessages<UserFinishedQueueingJobMessage>,
        IHandleMessages<UserSubmittedWorkItemMessage>,
        IHandleMessages<UserSubmittedHttpRequestMessage>
    {
        public class JobOrchestratorData : ContainSagaData
        {
            public Guid JobId { get; set; }
            public string JobName { get; set; }
            public int ExpectedWorkItemCount { get; set; }
            public int WorkItemCounter { get; set; }
            public bool IsFinishedQueueing { get; set; }
            public bool IsQueueing { get; set; }
            public bool IsJobStarted { get; set; }
            public bool IsJobCompleted { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<JobOrchestratorData> mapper)
        {
            mapper.ConfigureMapping<UserQueueingJobMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
            mapper.ConfigureMapping<UserFinishedQueueingJobMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
            mapper.ConfigureMapping<UserSubmittedWorkItemMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
            mapper.ConfigureMapping<UserSubmittedHttpRequestMessage>(message => message.JobId).ToSaga(sagaData => sagaData.JobId);
        }

        public async Task Handle(UserQueueingJobMessage message, IMessageHandlerContext context)
        {
            if(message.JobStatus == JobStatus.Queueing)
            {
                Data.IsQueueing = true;
                Data.ExpectedWorkItemCount = message.ExpectedWorkItemCount;
            }

            ILog log = LogManager.GetLogger<UserQueueingJobMessage>();
            JobRepository repo = new JobRepository();

            await repo.AddAsync(new JobEntity
            {
                Id = message.JobId,
                JobStatusId = (short)message.JobStatus,
                JobName = message.JobName
            });

            log.Info($"MESSAGE: {nameof(UserQueueingJobMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
        }

        public Task Handle(UserFinishedQueueingJobMessage message, IMessageHandlerContext context)
        {
            if (message.JobStatus == JobStatus.FinishedQueueing)
            {
                Data.IsFinishedQueueing = true;
                Data.IsQueueing = false;
            }

            ILog log = LogManager.GetLogger<UserFinishedQueueingJobMessage>();

            log.Info($"MESSAGE: {nameof(UserFinishedQueueingJobMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");

            MarkSagaAsComplete();

            return Task.CompletedTask;
        }

        public async Task Handle(UserSubmittedWorkItemMessage message, IMessageHandlerContext context)
        {
            Data.WorkItemCounter++;
            ILog log = LogManager.GetLogger<UserSubmittedWorkItemMessage>();

            log.Info($"MESSAGE: {nameof(UserSubmittedWorkItemMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");

            WorkItemRepository workItemRepository = new WorkItemRepository();

            await workItemRepository.AddAsync(new Core.Entities.WorkItemEntity
            {
                Id = message.WorkItemId,
                JobId = message.JobId,
                WorkItemStatusId = (short)WorkItemStatus.Started
            });

            foreach (var item in message.HttpRequests)
            {
                await context.SendLocal(new UserSubmittedHttpRequestMessage
                {
                    JobId = message.JobId,
                    WorkItemId = message.WorkItemId,
                    HttpRequest = item
                }).ConfigureAwait(false);
            }

            if(Data.ExpectedWorkItemCount == Data.WorkItemCounter)
            {
                Data.IsJobCompleted = true;
            }

            log.Info($"MESSAGE: {nameof(UserSubmittedWorkItemMessage)}; HANDLED BY: {nameof(JobSumittedHandler)}: {JsonConvert.SerializeObject(message)}");
        }

        public async Task Handle(UserSubmittedHttpRequestMessage message, IMessageHandlerContext context)
        {
           if(!Data.IsJobStarted)
            {
                Data.IsJobStarted = true;
            }

            ILog log = LogManager.GetLogger<UserSubmittedHttpRequestHandler>();

            HttpRequestRepository httpRequestRepository = new HttpRequestRepository();

            await httpRequestRepository.AddAsync(new HttpRequestEntity
            {
                Id = message.HttpRequest.HttpRequestId,
                WorkItemId = message.WorkItemId,
                Url = message.HttpRequest.Url,
                ContentType = message.HttpRequest.ContentType,
                Method = message.HttpRequest.Method,
                Content = message.HttpRequest.Content,
                RequestHeadersInJson = message.HttpRequest.RequestHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.RequestHeaders),
                ContentHeadersInJson = message.HttpRequest.ContentHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.ContentHeaders),
                AuthenticationConfigInJson = message.HttpRequest.AuthenticationConfig == null ? null : JsonConvert.SerializeObject(message.HttpRequest.AuthenticationConfig),
                HttpRequestStatusId = (short)HttpRequestStatus.Started
            });

            //issue http request

            //update satus to completed
            //await httpRequestRepository.UpdateAsync(new HttpRequestEntity
            //{
            //    Id = message.HttpRequest.HttpRequestId,
            //    HttpRequestStatusId = (short)HttpRequestStatus.Completed
            //});

            log.Info($"MESSAGE: {nameof(UserSubmittedHttpRequestMessage)}; HANDLED BY: {nameof(UserSubmittedHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");

            MarkSagaAsComplete();
        }

        private void MarkSagaAsComplete()
        {
            if (!Data.IsQueueing && Data.IsFinishedQueueing && Data.IsJobStarted && Data.IsJobCompleted)
            {
                Data.IsJobCompleted = true;
                MarkAsComplete();
            }
        }
    }
}
