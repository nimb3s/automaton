using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        /// <response code="201">Returns the newly created <see cref="WorkItemModel"/></response>
        /// <response code="400">When the jobid for this resource is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("api/automaton/{jobId}/[controller]")]
        public async Task<ActionResult> Post(Guid jobId, [FromBody] WorkItemModel workItem)
        {
            if (workItem.WorkItemStatus != WorkItemStatus.Queued)
            {
                return base.BadRequest(new
                {
                    workItem.WorkItemStatus,
                    WorkItemStatus_Error = $"You can only set this property to {Enum.GetName(typeof(WorkItemStatus), WorkItemStatus.Queued)}"
                });
            }

            workItem.JobId = jobId;
            workItem.WorkItemId = Guid.NewGuid();
            workItem.WorkItemStatus = WorkItemStatus.Queued;

            workItem.HttpRequests = new List<HttpRequestModel>
            {
                new HttpRequestModel
                {
                    HttpRequestId = Guid.NewGuid(),
                    WorkItemStatus = WorkItemStatus.Queued,
                    Url = "https://www.jw.org/en/",
                    Method = HttpMethods.Get,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new
                    {
                        Id = Guid.NewGuid(),
                        Status = "some status"
                    }),
                        AuthenticationConfig = new HttpAuthenticationConfig
                        {
                            AuthenticationType = HttpAuthenticationType.OAuth20,
                            AuthenticationOptions = new OAuth20AuthenticationConfig
                            {
                                Grant = new OAuth20ClientGrant
                                {
                                    ClientId = Guid.NewGuid().ToString(),
                                    ClientSecret = Guid.NewGuid().ToString()
                                }
                            }
                        }
                },
                new HttpRequestModel
                {
                    HttpRequestId = Guid.NewGuid(),
                    WorkItemStatus = WorkItemStatus.Queued,
                    Url = "https://www.cnn.com/",
                    Method = HttpMethods.Get,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new
                    {
                        Id = Guid.NewGuid(),
                        Status = "some status"
                    }),
                    AuthenticationConfig = new HttpAuthenticationConfig
                    {
                        AuthenticationType = HttpAuthenticationType.OAuth20,
                        AuthenticationOptions = new OAuth20AuthenticationConfig
                        {
                            Grant = new OAuth20PasswordGrant
                            {
                                ClientId = Guid.NewGuid().ToString(),
                                ClientSecret = Guid.NewGuid().ToString(),
                                Username = "some username",
                                UserPassword = "some password"
                            }
                        }
                    }
                },
                new HttpRequestModel
                {
                    HttpRequestId = Guid.NewGuid(),
                    WorkItemStatus = WorkItemStatus.Queued,
                    Url = "https://techcrunch.com/",
                    Method = HttpMethods.Get,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new
                    {
                        Id = Guid.NewGuid(),
                        Status = "some status"
                    }),
                    AuthenticationConfig = new HttpAuthenticationConfig
                    {
                        AuthenticationType = HttpAuthenticationType.None
                    }
                }
            };

            //TODO: if(automation job is not AutomationJobStatus.Queueing then reject the request
            //TODO: if client is trying to set automation job to finished queueing or done do not let them return forbidden?

            await messageSession.Send(new UserSubmittedWorkItemMessage
            {
                JobId = workItem.JobId,
                WorkItemId = workItem.WorkItemId,
                WorkItemStatus = workItem.WorkItemStatus,
                HttpRequests = workItem.HttpRequests.Select(i => new Request
                {
                    AuthenticationConfig = i.AuthenticationConfig,
                    Content = i.Content,
                    ContentHeaders = i.ContentHeaders,
                    ContentType = i.ContentType,
                    HttpRequestId = Guid.NewGuid(),
                    Method = i.Method,
                    RequestHeaders = i.RequestHeaders,
                    Url = i.Url,
                    HttpRequestStatus = HttpRequestStatus.Queued
                }).ToList()
            });

            return Created($"/api/automationjob/{workItem.JobId}/workitem/{workItem.WorkItemId}", workItem);
        }

        //// DELETE api/<AutomationRequest>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
