using Newtonsoft.Json;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class WorkItemSaga :
        Saga<WorkItemSaga.WorkItemSagaData>,

        IAmStartedByMessages<UserCreatedWorkItemMessage>,
        IAmStartedByMessages<UserRestartedWorkItemMessage>,
        IAmStartedByMessages<HttpRequestExecutedMessage>
    {
        static ILog log = LogManager.GetLogger<WorkItemSaga>();

        public class WorkItemSagaData : ContainSagaData
        {
            public Guid WorkItemId { get; set; }
            public int TotalRequests { get; set; }
            public int RequestCounter { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<WorkItemSaga.WorkItemSagaData> mapper)
        {
            mapper.ConfigureMapping<UserCreatedWorkItemMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
            mapper.ConfigureMapping<UserRestartedWorkItemMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
            mapper.ConfigureMapping<HttpRequestExecutedMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
        }

        public async Task Handle(UserCreatedWorkItemMessage message, IMessageHandlerContext context)
        {
            log.Info($"MESSAGE: {nameof(UserCreatedWorkItemMessage)}; HANDLED BY: {nameof(WorkItemSaga)}: {JsonConvert.SerializeObject(message)}");

            Data.TotalRequests = message.HttpRequests.Count();

            await context.SendLocal(new WorkItemCreatedMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequests = message.HttpRequests,
                CreateDate = message.DateActionTaken,
            }).ConfigureAwait(false);


            foreach (var item in message.HttpRequests)
            {
                await context.SendLocal(new ExecuteHttpRequestMessage
                {
                    JobId = message.JobId,
                    WorkItemId = message.WorkItemId,
                    HttpRequest = item,
                    CreateDate = DateTime.UtcNow
                }).ConfigureAwait(false);
            }
        }

        public async Task Handle(UserRestartedWorkItemMessage message, IMessageHandlerContext context)
        {
            log.Info($"MESSAGE: {nameof(UserRestartedWorkItemMessage)}; HANDLED BY: {nameof(WorkItemSaga)}: {JsonConvert.SerializeObject(message)}");

            Data.TotalRequests = message.HttpRequests.Count();

            await context.SendLocal(new WorkItemRestartedMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequests = message.HttpRequests,
                DateActionTaken = message.DateActionTaken,
            }).ConfigureAwait(false);


            foreach (var item in message.HttpRequests)
            {
                await context.SendLocal(new ExecuteHttpRequestMessage
                {
                    JobId = message.JobId,
                    WorkItemId = message.WorkItemId,
                    HttpRequest = item,
                    CreateDate = DateTime.UtcNow
                }).ConfigureAwait(false);
            }
        } 

        public async Task Handle(HttpRequestExecutedMessage message, IMessageHandlerContext context)
        {
            Data.RequestCounter++;
            
            //if data.requestconter <= 1 then raise workitem started message
            if(Data.TotalRequests == Data.RequestCounter)
            {
                await context.SendLocal(new WorkItemCompletedMessage
                {
                    JobId = message.JobId,
                    WorkItemId = message.WorkItemId,
                    HttpRequestId = message.HttpRequest.HttpRequestId,
                    DateActionTaken = message.DateActionTaken,
                }).ConfigureAwait(false);

                log.Info($"SAGA - MARKED AS COMPLETE:  MESSAGE - {nameof(HttpRequestExecutedMessage)}; HANDLED BY: {nameof(WorkItemSagaData)}");
                MarkAsComplete();
            }
        }
    }
}
