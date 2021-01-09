using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nimb3s.Automaton.Api.Models;
using Nimb3s.Automaton.Messages.User;
using NServiceBus;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nimb3s.Automaton.Api.Controllers
{
    [ApiController]
    public class AutomationJobController : ControllerBase
    {
        private readonly IMessageSession messageSession;

        public AutomationJobController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        /// <summary>
        /// Creates a <see cref="JobModel"/> item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /asfd
        ///     {
        ///     }
        /// </remarks>
        /// <response code="200">Returns ok when the automation job is reset to <see cref="JobStatus.Queueing"/> or <see cref="JobStatus.Started"/> </response>
        /// <response code="201">Returns the newly created <see cref="JobModel"/></response>
        /// <response code="400">If the item is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("api/automaton/jobs")]
        public async Task<ActionResult> Post([FromBody] JobModel job)
        {
            if(job.JobStatus != JobStatus.Queueing)
            {
                return BadRequest(new
                {
                    job.JobStatus,
                    WorkItemStatus_Error = $"You can only set this property to {Enum.GetName(typeof(JobStatus), JobStatus.Queueing)}"
                });
            }

            job.JobId = Guid.NewGuid();

            //TODO: if(automation job is not AutomationJobStatus.Queueing then reject the request
            //TODO: if client is trying to set automation job to finished queueing or done do not let them return forbidden?

            
            await messageSession.Send(new UserQueueingJobMessage
            {
                JobId = job.JobId,
                JobName = job.Name
            });

            return Created($"/api/automationjob/{job.JobId}", job);
        }

        // PUT api/<AutomationRequest>/asdf-asdf-asdf-asdf
        /// <summary>
        /// Creates a <see cref="WorkItemModel"/> item.
        /// </summary>
        /// <response code="201">Returns the newly created <see cref="WorkItemModel"/></response>
        /// <response code="400">When the jobid for this resource is not found</response> 
        ///
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("api/automaton/jobs/{jobId}")]
        public async Task<ActionResult> Put(string jobId, [FromBody] JobModel job)
        {
            if (job.JobStatus != JobStatus.FinishedQueueing)
            {
                return BadRequest(new
                {
                    job.JobStatus,
                    WorkItemStatus_Error = $"You can only set this property to {Enum.GetName(typeof(JobStatus), JobStatus.FinishedQueueing)}"
                });
            }

            await messageSession.Send(new UserFinishedQueueingJobMessage
            {
                JobId = job.JobId,
            });

            return Created($"/api/automaton/jobs/{job.JobId}", job);
        }

        //// DELETE api/<AutomationRequest>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
