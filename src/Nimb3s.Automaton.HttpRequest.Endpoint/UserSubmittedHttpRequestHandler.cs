﻿using Newtonsoft.Json;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Automaton.Messages;
using Nimb3s.Automaton.Messages.HttpRequests;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.HttpRequest.Endpoint
{
    public class UserSubmittedHttpRequestHandler : IHandleMessages<UserSubmittedHttpRequestMessage>
    {
        static ILog log = LogManager.GetLogger<UserSubmittedHttpRequestHandler>();

        #region MessageHandler
        public async Task Handle(UserSubmittedHttpRequestMessage message, IMessageHandlerContext context)
        {
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
                HttpRequestStatusId = (short)message.HttpRequest.HttpRequestStatus
            });

            log.Info($"MESSAGE: {nameof(UserSubmittedHttpRequestMessage)}; HANDLED BY: {nameof(UserSubmittedHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");

            context.Send(new )
        }
        #endregion
    }
}
