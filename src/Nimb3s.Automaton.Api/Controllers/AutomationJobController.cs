using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nimb3s.Automaton.Api.Models;
using Nimb3s.Automaton.Messages;
using Nimb3s.Automaton.Messages.Jobs;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nimb3s.Automaton.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutomationJobController : ControllerBase
    {
        private readonly IMessageSession messageSession;

        public AutomationJobController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        /// <summary>
        /// Creates a <see cref="AutomationJobModel"/> item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /asfd
        ///     {
        ///     }
        /// </remarks>
        /// <response code="200">Returns ok when the automation job is reset to <see cref="JobStatus.Queueing"/> or <see cref="JobStatus.Started"/> </response>
        /// <response code="201">Returns the newly created <see cref="AutomationJobModel"/></response>
        /// <response code="400">If the item is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutomationJobModel automationJob)
        {
            if(automationJob.AutomationJobStatus != JobStatus.Queueing)
            {
                return BadRequest(new
                {
                    automationJob.AutomationJobStatus,
                    WorkItemStatus_Error = $"You can only set this property to {Enum.GetName(typeof(JobStatus), JobStatus.Queueing)}"
                });
            }

            automationJob.JobId = Guid.NewGuid();

            //TODO: if(automation job is not AutomationJobStatus.Queueing then reject the request
            //TODO: if client is trying to set automation job to finished queueing or done do not let them return forbidden?

            
            await messageSession.Send(new UserSubmittedAutomationJobMessage
            {
                JobId = automationJob.JobId,
                JobName = automationJob.Name,
                JobStatus = automationJob.AutomationJobStatus
            });

            return Created($"/api/automationjob/{automationJob.JobId}", automationJob);
        }

        /*
        // PUT api/<AutomationRequest>/asdf-asdf-asdf-asdf
        /// <summary>
        /// Creates a <see cref="WorkItemModel"/> item.
        /// </summary>
        /// <response code="201">Returns the newly created <see cref="WorkItemModel"/></response>
        /// <response code="400">When the jobid for this resource is not found</response> 
        ///
        */
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPut("{automationJobId}")]
        //public void Put(string automationJobId, [FromBody] WorkItemModel httpRequest)
        //{
            
        //}

        //// DELETE api/<AutomationRequest>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
