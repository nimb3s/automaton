using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserRestartedJobMessage : IMessage
    {
        public Guid JobId { get; set; }
        public DateTimeOffset DateActionTookPlace { get; set; }
    }
}
