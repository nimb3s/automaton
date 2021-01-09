using Nimb3s.Automaton.Messages.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.Job
{
    public class ExecuteHttpRequestMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public Request HttpRequest { get; set; }
    }
}
