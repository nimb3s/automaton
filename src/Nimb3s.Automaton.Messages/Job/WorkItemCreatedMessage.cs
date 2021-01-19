using Nimb3s.Automaton.Pocos;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Messages.Job
{
    public class WorkItemCreatedMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public IEnumerable<UserHttpRequest> HttpRequests { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
