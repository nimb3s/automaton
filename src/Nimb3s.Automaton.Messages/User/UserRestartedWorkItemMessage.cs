using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserRestartedWorkItemMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public IEnumerable<UserHttpRequest> HttpRequests { get; set; }
        public DateTimeOffset DateActionTaken { get; set; }
    }
}
