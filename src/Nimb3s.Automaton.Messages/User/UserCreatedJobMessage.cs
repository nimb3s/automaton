using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserCreatedJobMessage : IMessage
    {
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public DateTimeOffset DateActionTookPlace { get; set; }
    }

    /// <summary>
    /// The status of the automation request.
    /// <see cref="JobStatus.Created"/>:
    /// This status indicates that subsequent endpoint requests should be grouped together. This status is used to support
    /// grouping multiple endpoint requests into a sigle automation job.
    /// <see cref="JobStatus.FinishedQueueing"/>:
    /// This status indicates that endpoint requests will no longer be accepted as part of this job. If a client wishes to only process
    /// one  endpoint request, the <see cref="JobStatus.FinishedQueueing"/> should be used.
    /// <see cref="JobStatus.Started"/>:
    /// After job items have <see cref="JobStatus.FinishedQueueing"/> and all items are <see cref="JobStatus.Completed"/>
    /// processing, a job can be restarted.
    /// <see cref="JobStatus.Completed"/>:
    /// The status is set to <see cref="JobStatus.Completed"/> after all endpoints of a job have finished processsing.
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// When a job is created by a user.
        /// </summary>
        Created,

        /// <summary>
        /// This status indicates that endpoint requests will no longer be accepted as part of this job. If a client wishes to only process
        /// one  endpoint request, the <see cref="JobStatus.FinishedQueueing"/> should be used.
        /// </summary>
        FinishedQueueing,

        /// <summary>
        /// After job items have <see cref="JobStatus.FinishedQueueing"/> and all items are <see cref="JobStatus.Completed"/>
        /// processing, a job can be restarted.
        /// </summary>
        Started,

        /// <summary>
        /// The status is set to <see cref="JobStatus.Restart"/> when client wants the entire job to run again.
        /// </summary>
        Restart,
    }
}
