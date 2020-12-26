using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages
{
    public class UserSubmittedWorkItemMessage : IMessage
    {
        public string AutomationJobId { get; set; }
        public string WorkItemId { get; set; }
        public string WorkItemStatus { get; set; }
    }
}
