using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.Jobs
{
    public class UserFinishedQueueingJobMessage: IMessage
    {
        public Guid JobId { get; set; }
        public JobStatus JobStatus { get; set; }
    }
}
