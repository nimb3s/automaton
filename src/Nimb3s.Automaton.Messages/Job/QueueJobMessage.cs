using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.Job
{
    public class QueueJobMessage
    {
        public Guid JobId { get; set; }
        public JobStatus Status { get; set; }
        public string Name { get; set; }
    }
}
