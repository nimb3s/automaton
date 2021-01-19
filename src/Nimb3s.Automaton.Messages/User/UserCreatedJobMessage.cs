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
    /// <see cref="JobStatusType.Created"/>:
    /// This status indicates that subsequent endpoint requests should be grouped together. This status is used to support
    /// grouping multiple endpoint requests into a sigle automation job.
    /// <see cref="JobStatusType.FinishedQueueing"/>:
    /// This status indicates that endpoint requests will no longer be accepted as part of this job. If a client wishes to only process
    /// one  endpoint request, the <see cref="JobStatusType.FinishedQueueing"/> should be used.
    /// <see cref="JobStatusType.Started"/>:
    /// After job items have <see cref="JobStatusType.FinishedQueueing"/> and all items are <see cref="JobStatusType.Completed"/>
    /// processing, a job can be restarted.
    /// <see cref="JobStatusType.Completed"/>:
    /// The status is set to <see cref="JobStatusType.Completed"/> after all endpoints of a job have finished processsing.
    /// </summary>
    public enum JobStatusType
    {
        /// <summary>
        /// When a job is created by a user.
        /// </summary>
        Created = 1,

        /// <summary>
        /// This status indicates that endpoint requests will no longer be accepted as part of this job. If a client wishes to only process
        /// one  endpoint request, the <see cref="JobStatusType.FinishedQueueing"/> should be used.
        /// </summary>
        FinishedQueueing = 2,

        /// <summary>
        /// After job items have <see cref="JobStatusType.FinishedQueueing"/> and all items are <see cref="JobStatusType.Completed"/>
        /// processing, a job can be restarted.
        /// </summary>
        Started = 3,

        /// <summary>
        /// The status is set to <see cref="JobStatusType.Restart"/> when client wants the entire job to run again.
        /// </summary>
        Restart = 4,
    }
}
