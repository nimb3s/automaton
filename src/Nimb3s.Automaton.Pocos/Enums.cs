namespace Nimb3s.Automaton.Pocos
{
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

    public enum GrantType
    {
        /// <summary>
        /// Supports client id and client secret: client_credentials
        /// </summary>
        ClientCredentials = 1,
        /// <summary>
        /// Supports client id and client secret with username and user password: password
        /// </summary>
        Password = 2
    }
    public enum WorkItemStatusType
    {
        /// <summary>
        /// When a work item is queued.
        /// </summary>
        Queued = 1,

        /// <summary>
        /// When a work item starts running.
        /// </summary>
        Started = 2,

        /// <summary>
        /// When a work item starts running.
        /// </summary>
        ReStarted = 3,

        /// <summary>
        /// When a work item completes.
        /// </summary>
        Completed = 4
    }
    public enum HttpRequestStatusType
    {
        Queued = 1,
        Started = 2,
        Completed = 3
    }
    public enum HttpAuthenticationType
    {
        None = 1,
        OAuth20 = 2
    }

    public enum HttpRequestContentType
    {
        ApplicationFormUrlEncoded = 1,
    }
}
