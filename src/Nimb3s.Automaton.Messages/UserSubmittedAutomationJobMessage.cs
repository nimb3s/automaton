using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages
{
    public class UserSubmittedAutomationJobMessage : IMessage
    {
        public Guid AutomationJobId { get; set; }
        public string AutomationJobName { get; set; }
        public AutomationJobStatus AutomationJobStatus { get; set; }
    }

    /// <summary>
    /// The status of the automation request.
    /// <see cref="AutomationJobStatus.Queueing"/>:
    /// This status indicates that subsequent endpoint requests should be grouped together. This status is used to support
    /// grouping multiple endpoint requests into a sigle automation job.
    /// <see cref="AutomationJobStatus.FinishedQueueing"/>:
    /// This status indicates that endpoint requests will no longer be accepted as part of this job. If a client wishes to only process
    /// one  endpoint request, the <see cref="AutomationJobStatus.FinishedQueueing"/> should be used.
    /// <see cref="AutomationJobStatus.Started"/>:
    /// After job items have <see cref="AutomationJobStatus.FinishedQueueing"/> and all items are <see cref="AutomationJobStatus.Completed"/>
    /// processing, a job can be restarted.
    /// <see cref="AutomationJobStatus.Completed"/>:
    /// The status is set to <see cref="AutomationJobStatus.Completed"/> after all endpoints of a job have finished processsing.
    /// </summary>
    public enum AutomationJobStatus
    {
        /// <summary>
        /// This status indicates that subsequent endpoint requests should be grouped together. This status is used to support
        /// grouping multiple endpoint requests into a sigle automation job.
        /// </summary>
        Queueing,

        /// <summary>
        /// This status indicates that endpoint requests will no longer be accepted as part of this job. If a client wishes to only process
        /// one  endpoint request, the <see cref="AutomationJobStatus.FinishedQueueing"/> should be used.
        /// </summary>
        FinishedQueueing,

        /// <summary>
        /// After job items have <see cref="AutomationJobStatus.FinishedQueueing"/> and all items are <see cref="AutomationJobStatus.Completed"/>
        /// processing, a job can be restarted.
        /// </summary>
        Started,

        /// <summary>
        /// The status is set to <see cref="AutomationJobStatus.Completed"/> after all endpoints of a job have finished processsing.
        /// </summary>
        Completed,

    }
}
