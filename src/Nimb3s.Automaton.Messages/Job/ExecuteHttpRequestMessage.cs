using Nimb3s.Automaton.Pocos;
using System;

namespace Nimb3s.Automaton.Messages.Job
{
    public class ExecuteHttpRequestMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public UserHttpRequest HttpRequest { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
