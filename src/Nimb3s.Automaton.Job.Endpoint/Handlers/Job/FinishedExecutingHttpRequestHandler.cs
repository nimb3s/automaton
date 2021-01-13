using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
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
    public class FinishedExecutingHttpRequestHandler : IHandleMessages<FinishedExecutingHttpRequestMessage>
    {
        static ILog log = LogManager.GetLogger<UserFinishedQueueingJobHandler>();

        #region MessageHandler
        public async Task Handle(FinishedExecutingHttpRequestMessage message, IMessageHandlerContext context)
        {
            //AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            //await dbContext.HttpRequestStatusRepository.UpsertAsync(new HttpRequestStatusEntity
            //{
            //    HttpRequestId = message.HttpRequestId,
            //    HttpRequestStatusTypeId = (short)HttpRequestStatus.Completed,
            //    StatusTimeStamp = DateTimeOffset.UtcNow,
            //});

            //dbContext.Commit();

            log.Info($"MESSAGE: {nameof(FinishedExecutingHttpRequestMessage)}; HANDLED BY: {nameof(FinishedExecutingHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");
        }
        #endregion
    }
}
