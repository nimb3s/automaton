using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserCreatedJobMessage : IMessage
    {
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public DateTimeOffset DateActionTookPlace { get; set; }
    }
}
