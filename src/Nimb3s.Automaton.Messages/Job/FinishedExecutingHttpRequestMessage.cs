using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.Job
{
    public class FinishedExecutingHttpRequestMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public Guid HttpRequestId { get; set; }
    }
}
