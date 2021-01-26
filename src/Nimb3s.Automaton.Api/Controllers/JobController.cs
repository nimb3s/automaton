using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nimb3s.Automaton.Messages.User;
using Nimb3s.Automaton.Pocos;
using Nimb3s.Automaton.Pocos.Models;
using NServiceBus;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nimb3s.Automaton.Api.Controllers
{
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IMessageSession messageSession;

        public JobController(IMessageSession messageSession)
        {
            this.messageSession = messageSession;
        }

        /// <summary>
        /// Creates a <see cref="JobCreatedModel"/> item.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /asfd
        ///     {
        ///     }
        /// </remarks>
        /// <response code="200">Returns ok when the automation job is reset to <see cref="JobStatusType.Created"/> or <see cref="JobStatusType.Started"/> </response>
        /// <response code="201">Returns the newly created <see cref="JobCreatedModel"/></response>
        /// <response code="400">If the item is not found</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("api/automaton/jobs")]
        public async Task<ActionResult> Post([FromBody] JobCreatedModel job)
        {
            Guid jobId = Guid.NewGuid();

            await messageSession.Send(new UserCreatedJobMessage
            {
                JobId = jobId,
                JobName = job.Name,
                DateActionTookPlace = DateTime.UtcNow
            });

            return Created($"/api/automaton/jobs/{jobId}", new NewJobCreatedModel
            {
                JobId = jobId,
                JobStatus = JobStatusType.Created,
                Name = job.Name
            });
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
        public async Task<ActionResult> Put(Guid jobId, [FromBody] UpdateJobModel job)
        {
            ActionResult actionResult = null;

            switch (job.JobStatus)
            {
                case JobStatusType.Created:
                case JobStatusType.Started:
                    actionResult =  BadRequest(new
                    {
                        job.JobStatus,
                        JobStatus_Error = $"You can only set this property to {Enum.GetName(typeof(JobStatusType), JobStatusType.FinishedQueueing)} or {Enum.GetName(typeof(JobStatusType), JobStatusType.Restart)}"
                    });
                    break;
                case JobStatusType.FinishedQueueing:
                    await messageSession.Send(new UserFinishedQueueingJobMessage
                    {
                        JobId = jobId,
                        ActionTookPlaceDate = DateTimeOffset.UtcNow
                    });
                    actionResult = Created($"/api/automaton/jobs/{jobId}", job);
                    break;
                case JobStatusType.Restart:
                    await messageSession.Send(new UserRestartedJobMessage
                    {
                        JobId = jobId,
                        DateActionTookPlace = DateTimeOffset.UtcNow
                    });
                    actionResult = Created($"/api/automaton/jobs/{jobId}", job);
                    break;
                default:
                    actionResult = NotFound();
                    break;
            }

            return actionResult;
        }

        //// DELETE api/<AutomationRequest>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
