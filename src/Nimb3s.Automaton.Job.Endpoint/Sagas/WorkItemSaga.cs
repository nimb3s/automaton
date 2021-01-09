using Nimb3s.Automaton.Messages.Job;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class WorkItemSaga :
        Saga<WorkItemSaga.WorkItemSagaData>,

        IAmStartedByMessages<CreateWorkItemMessage>,
        IAmStartedByMessages<HttpRequestExecutedMessage>
    {
        public class WorkItemSagaData : ContainSagaData
        {
            public Guid WorkItemId { get; set; }
            public int TotalRequests { get; set; }
            public int RequestCounter { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<WorkItemSaga.WorkItemSagaData> mapper)
        {
            mapper.ConfigureMapping<CreateWorkItemMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
            mapper.ConfigureMapping<HttpRequestExecutedMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
        }

        public async Task Handle(CreateWorkItemMessage message, IMessageHandlerContext context)
        {
            Data.TotalRequests = message.HttpRequests.Count();

            await context.SendLocal(new WorkItemCreatedMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequests = message.HttpRequests
            }).ConfigureAwait(false);


            foreach (var item in message.HttpRequests)
            {
                await context.SendLocal(new ExecuteHttpRequestMessage
                {
                    JobId = message.JobId,
                    WorkItemId = message.WorkItemId,
                    HttpRequest = item
                }).ConfigureAwait(false);
            }
        }

        public Task Handle(HttpRequestExecutedMessage message, IMessageHandlerContext context)
        {
            Data.RequestCounter++;
            
            if(Data.TotalRequests == Data.RequestCounter)
            {
                MarkAsComplete();
            }

            return Task.CompletedTask;
        }
    }
}
