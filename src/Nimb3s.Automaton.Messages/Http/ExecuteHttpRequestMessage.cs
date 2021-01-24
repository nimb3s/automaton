using Nimb3s.Automaton.Pocos;
using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.HttpRequest
{
    public class ExecuteHttpRequestMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public UserHttpRequest HttpRequest { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
