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

        IAmStartedByMessages<WorkItemCreatedMessage>,
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
            mapper.ConfigureMapping<WorkItemCreatedMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
            mapper.ConfigureMapping<HttpRequestExecutedMessage>(message => message.WorkItemId).ToSaga(sagaData => sagaData.WorkItemId);
        }

        public async Task Handle(WorkItemCreatedMessage message, IMessageHandlerContext context)
        {
            Data.TotalRequests = message.HttpRequests.Count();

            foreach (var item in message.HttpRequests)
            {
                await context.SendLocal(new HttpRequestExecutedMessage
                {
                    JobId = message.JobId,
                    WorkItemId = message.WorkItemId,
                    HttpRequest = item
                }).ConfigureAwait(false);
            }
        }
    }
}
