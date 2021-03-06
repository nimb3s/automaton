﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nimb3s.Automaton.Core.Repositories.Sql;
using Nimb3s.Automaton.Messages.User;
using Nimb3s.Automaton.Pocos;
using Nimb3s.Automaton.Pocos.Models;
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
        private readonly IAutomatonInMemoryContext _inMemoryDbContext;
        private readonly IMessageSession messageSession;

        public WorkItemController(IAutomatonInMemoryContext inMemoryDbContext, IMessageSession messageSession)
        {
            _inMemoryDbContext = inMemoryDbContext;
            this.messageSession = messageSession;
        }

        /// <summary>
        /// Gets work item status using the job id.
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  GET api/automaton/jobId/workitem
        ///
        /// </remarks>
        /// <returns>returns a <see cref="WorkItemStatusModel"/></returns>
        /// <response code="200">Returns the work item status model if job id is found</response>
        /// <response code="400">If the item is not found</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("api/automaton/{jobId}/workitem")]
        public async Task<ActionResult> GetWorkItemStatus(Guid jobId)
        {
            var workItem = await _inMemoryDbContext.WorkItemStatusInMemoryRepository.Get(jobId);
            var workItemStatusNum = workItem.WorkItemStatusTypeId;

            return Ok(new WorkItemStatusModel 
            {
                WorkItemStatus = (WorkItemStatusType)workItemStatusNum
            });

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
        /// <response code="201">Returns the newly created <see cref="WorkItemCreatedModel"/></response>
        /// <response code="400">When the jobid for this resource is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("api/automaton/{jobId}/[controller]")]
        public async Task<ActionResult> Post(string jobId, [FromBody] NewWorkItemModel newWorkItem)
        {
            Guid workItemId = Guid.NewGuid();
            newWorkItem.JobId = Guid.Parse(jobId);

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

            return Created($"/api/automaton/job/{newWorkItem.JobId}/workitem/{workItemId}", new WorkItemCreatedModel
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
