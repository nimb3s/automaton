using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nimb3s.Automaton.Api.Models;
using Nimb3s.Automaton.Messages;
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


        // POST api/automationjob/{automationJobId}/[controller]
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
        [HttpPost("api/automationjob/{automationJobId}/[controller]")]
        public async Task<ActionResult> Post(string automationJobId, [FromBody] WorkItemModel workItem)
        {
            if (workItem.WorkItemStatus != WorkItemStatus.Queued)
            {
                return BadRequest(new
                {
                    workItem.WorkItemStatus,
                    WorkItemStatus_Error = $"You can only set this property to {Enum.GetName(typeof(WorkItemStatus), WorkItemStatus.Queued)}"
                });
            }

            workItem.AutomationJobId = automationJobId;
            workItem.WorkItemId = Guid.NewGuid().ToString();
            workItem.WorkItemStatus = WorkItemStatus.Queued;

            //TODO: if(automation job is not AutomationJobStatus.Queueing then reject the request
            //TODO: if client is trying to set automation job to finished queueing or done do not let them return forbidden?

            await messageSession.Send(new UserSubmittedWorkItemMessage
            {
                AutomationJobId = workItem.AutomationJobId,
                WorkItemId = workItem.WorkItemId,
                WorkItemStatus = Enum.GetName(typeof(WorkItemStatus), workItem.WorkItemStatus)
            });

            return Created($"/api/automationjob/{workItem.AutomationJobId}/workitem/{workItem.WorkItemId}", workItem);
        }

        //// DELETE api/<AutomationRequest>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
