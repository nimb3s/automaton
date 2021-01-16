using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nimb3s.Automaton.Api.Models;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nimb3s.Automaton.Api.Controllers
{
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IMessageSession messageSession;

        public WorkItemController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        // POST api/automationjob/{jobId}/[controller]
        /// <summary>
        /// Creates a <see cref="WorkItemModel"/> item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /asfd
        ///     {
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created <see cref="CreatedWorkItemModel"/></response>
        /// <response code="400">When the jobid for this resource is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("api/automaton/{jobId}/[controller]")]
        public async Task<ActionResult> Post(string jobId, [FromBody] NewWorkItemModel newWorkItem)
        {
            Guid workItemId = Guid.NewGuid();

            newWorkItem.JobId = Guid.Parse(jobId);

            newWorkItem.HttpRequests = new List<NewHttpRequestModel>
            {
                new NewHttpRequestModel
                {
                    Url = "https://test.lightstream.com/api/user/accounts",
                    Method = HttpMethods.Get,
                    ContentType = "application/json",
                    AuthenticationConfig = new HttpAuthenticationConfig
                    {
                        AuthenticationType = HttpAuthenticationType.OAuth20,
                        AuthenticationOptions = new OAuth20AuthenticationConfig
                        {
                            Grant = new OAuth20PasswordGrant
                            {
                                ClientId = "A050C5D7-58F7-4A17-8E96-937B47A77D1E",
                                ClientSecret = "21B5F798-BE55-42BC-8AA8-0025B903DC3B",
                                GrantType = GrantType.Password,
                                Scopes = "openid offline_access https://www.lightstream.com/auth/user",
                                Username = "Tester9040344591131",
                                UserPassword = "password1",
                            },
                            HttpRequestConfig = new OAuth20HttpRequestConfig
                            {
                                AuthUrl = "https://test.lightstream.com/connect/token",
                                ContentType = HttpRequestContentType.ApplicationFormUrlEncoded,
                                RevocationUrl = "https://test.lightstream.com/connect/revocation"
                            }
                        }
                    }
                }
            };

            var wi = JsonConvert.SerializeObject(newWorkItem, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            //TODO: if(automation job is not AutomationJobStatus.Queueing then reject the request
            //TODO: if client is trying to set automation job to finished queueing or done do not let them return forbidden?

            UserCreatedWorkItemMessage message = new UserCreatedWorkItemMessage
            {
                JobId = newWorkItem.JobId,
                WorkItemId = workItemId,
                HttpRequests = newWorkItem.HttpRequests.Select(i => new UserHttpRequest
                {
                    AuthenticationConfig = i.AuthenticationConfig,
                    Content = i.Content,
                    UserAgent = i.UserAgent,
                    ContentHeaders = i.ContentHeaders,
                    ContentType = i.ContentType,
                    HttpRequestId = Guid.NewGuid(),
                    Method = i.Method,
                    RequestHeaders = i.RequestHeaders,
                    Url = i.Url,
                }).ToList(),
                DateActionTaken = DateTimeOffset.UtcNow
            };

            await messageSession.Send(message);

            return Created($"/api/automaton/job/{newWorkItem.JobId}/workitem/{workItemId}", new CreatedWorkItemModel
            {
                JobId = message.JobId,
                WorkItemId = message.WorkItemId,
                HttpRequests = message.HttpRequests.Select(i => new CreatedHttpRequestModel
                {
                    HttpRequestId = i.HttpRequestId,
                    AuthenticationConfig = i.AuthenticationConfig,
                    Content = i.Content,
                    UserAgent = i .UserAgent,
                    ContentHeaders = i.ContentHeaders,
                    ContentType = i.ContentType,
                    Method = i.Method,
                    RequestHeaders = i.RequestHeaders,
                    Url = i.Url,
                    HttpRequestStatus = HttpRequestStatusType.Queued,
                }),
                WorkItemStatus = WorkItemStatusType.Queued
            });
        }

        //// DELETE api/<AutomationRequest>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
