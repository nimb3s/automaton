using Newtonsoft.Json;
using Nimb3s.Automaton.Core;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Job.Endpoint.Factories;
using Nimb3s.Automaton.Messages.Job;
using Nimb3s.Automaton.Pocos;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Job.Endpoint
{
    public class ExecuteHttpRequestHandler : IHandleMessages<ExecuteHttpRequestMessage>
    {
        static ILog log = LogManager.GetLogger<ExecuteHttpRequestHandler>();

        public async Task Handle(ExecuteHttpRequestMessage message, IMessageHandlerContext context)
        {
            await SetStatusToStarted(message);
            var httpResponseMessage = await SendHttpRequestAsync(message);

            await SaveHttpRequestAsync(message, httpResponseMessage);
            
            await context.SendLocal(new HttpRequestExecutedMessage
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequest = message.HttpRequest,
                DateActionTaken = DateTime.UtcNow
            });

            log.Info($"MESSAGE: {nameof(ExecuteHttpRequestMessage)}; HANDLED BY: {nameof(ExecuteHttpRequestHandler)}: {JsonConvert.SerializeObject(message)}");
        }

        private async Task SetStatusToStarted(ExecuteHttpRequestMessage message)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();

            await dbContext.HttpRequestRepository.UpsertAsync(new HttpRequestEntity
            {
                Id = message.HttpRequest.HttpRequestId,
                WorkItemId = message.WorkItemId,
                Url = message.HttpRequest.Url,
                ContentType = message.HttpRequest.ContentType,
                Method = message.HttpRequest.Method,
                Content = message.HttpRequest.Content,
                UserAgent = message.HttpRequest.UserAgent,
                RequestHeadersInJson = message.HttpRequest.RequestHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.RequestHeaders),
                ContentHeadersInJson = message.HttpRequest.ContentHeaders == null ? null : JsonConvert.SerializeObject(message.HttpRequest.ContentHeaders),
                AuthenticationConfigInJson = message.HttpRequest.AuthenticationConfig == null ? null : JsonConvert.SerializeObject(message.HttpRequest.AuthenticationConfig),
            });

            await dbContext.HttpRequestStatusRepository.UpsertAsync(new HttpRequestStatusEntity
            {
                HttpRequestId = message.HttpRequest.HttpRequestId,
                HttpRequestStatusTypeId = (short)HttpRequestStatusType.Started,
                StatusTimeStamp = DateTimeOffset.UtcNow,
            });

            dbContext.Commit();
        }

        private async Task<HttpResponseMessage> SendHttpRequestAsync(ExecuteHttpRequestMessage message)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                var authRequest = await HttpAuthRequestFactory.Create(message.HttpRequest);
               
                SetHttpMethod(authRequest.HttpRequestMessage, message.HttpRequest.Method);
                AddRequestHeaders(authRequest.HttpRequestMessage, message.HttpRequest.RequestHeaders, message.HttpRequest.UserAgent);
                AddContentHeaders(authRequest.HttpRequestMessage, message.HttpRequest.RequestHeaders);
                
                var response = await SendAsync(authRequest.HttpRequestMessage, message.HttpRequest);

                await HandleSignOutsAsync(message.HttpRequest.AuthenticationConfig, authRequest.AuthResponse);

                return response;
            }
            catch (Exception ex)
            {
                httpResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                httpResponse.Content = new StringContent($"Exception: {ex.Message}. StackTrace: {ex.StackTrace}");
            }

            return httpResponse;
        }

        private void SetHttpMethod(HttpRequestMessage httpRequestMessage, string httpMethod)
        {
            switch (httpMethod)
            {
                case Constants.Http.HTTP_METHOD_GET:
                    httpRequestMessage.Method = HttpMethod.Get;
                    break;
                case Constants.Http.HTTP_METHOD_POST:
                    httpRequestMessage.Method = HttpMethod.Post;
                    break;
                case Constants.Http.HTTP_METHOD_DELETE:
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
                case Constants.Http.HTTP_METHOD_PUT:
                    httpRequestMessage.Method = HttpMethod.Put;
                    break;
                default:
                    //TODO: throw exception when method type not found
                    break;
            }
        }

        private void AddRequestHeaders(HttpRequestMessage httpRequestMessage, Dictionary<string, string> headers, string userAgent)
        {
            if (userAgent != null)
            {
                httpRequestMessage.Headers.UserAgent.TryParseAdd(userAgent);
            }
            else
            {
                httpRequestMessage.Headers.UserAgent.TryParseAdd(Constants.Http.DEFAULT_USER_AGENT);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        private void AddContentHeaders(HttpRequestMessage httpRequestMessage, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpRequestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, UserHttpRequest userHttpRequest)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = null;

            switch (httpRequestMessage.Method.Method.ToLower())
            {
                case Constants.Http.HTTP_METHOD_GET:
                    response = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
                    break;
                case Constants.Http.HTTP_METHOD_POST:
                    httpRequestMessage.Content = new StringContent(userHttpRequest.Content, Encoding.UTF8, userHttpRequest.ContentType);
                    response = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
                    break;
                case Constants.Http.HTTP_METHOD_DELETE:
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
                case Constants.Http.HTTP_METHOD_PUT:
                    httpRequestMessage.Method = HttpMethod.Put;
                    break;
                default:
                    //TODO: throw exception when method type not found
                    break;
            }

            return response;
        }

        private async Task SaveHttpRequestAsync(ExecuteHttpRequestMessage message, HttpResponseMessage httpResponseMessage)
        {
            AutomatonDatabaseContext dbContext = new AutomatonDatabaseContext();
            var content = await httpResponseMessage.Content?.ReadAsStringAsync();

            await dbContext.HttpResponseRepository.UpsertAsync(new HttpResponseEntity
            {
                Id = Guid.NewGuid(),
                HttpRequestId = message.HttpRequest.HttpRequestId,
                StatusCode = (int)httpResponseMessage.StatusCode,
                Body = content,
                InsertTimeStamp = message.CreateDate
            });

            await dbContext.HttpRequestStatusRepository.UpsertAsync(new HttpRequestStatusEntity
            {
                HttpRequestId = message.HttpRequest.HttpRequestId,
                HttpRequestStatusTypeId = (short)HttpRequestStatusType.Completed,
                StatusTimeStamp = DateTimeOffset.UtcNow,
            });

            dbContext.Commit();
        }

        private async Task HandleSignOutsAsync(HttpAuthenticationConfig authenticationConfig, AuthResponseBase authResponse)
        {
            switch (authenticationConfig.AuthenticationType)
            {
                case HttpAuthenticationType.None:
                    break;
                case HttpAuthenticationType.OAuth20:
                    await HttpAuthRequestFactory.SignOut((OAuth20AuthenticationConfig)authenticationConfig.AuthenticationOptions, (OAuth20AuthResponse)authResponse);
                    break;
                default:
                    break;
            }
        }
    }
}
