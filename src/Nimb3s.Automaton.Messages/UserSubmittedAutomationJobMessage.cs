using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages
{
    public class UserSubmittedAutomationJobMessage : IMessage
    {
        public Guid AutomationJobId { get; set; }
        public string AutomationJobName { get; set; }
        public string AutomationJobStatus { get; set; }
    }
}
