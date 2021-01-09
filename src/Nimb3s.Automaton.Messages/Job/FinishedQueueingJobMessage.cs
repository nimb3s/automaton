using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.Job
{
    public class FinishedQueueingJobMessage
    {
        public Guid JobId { get; set; }
        public JobStatus JobStatus { get; set; }
    }
}
