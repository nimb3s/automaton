using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages
{
    public class UserSubmittedWorkItemMessage : IMessage
    {
        public Guid AutomationJobId { get; set; }
        public Guid WorkItemId { get; set; }
        public short WorkItemStatusId { get; set; }
    }
}
