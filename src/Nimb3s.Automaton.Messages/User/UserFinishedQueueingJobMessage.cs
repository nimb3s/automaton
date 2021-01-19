using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserFinishedQueueingJobMessage: IMessage
    {
        public Guid JobId { get; set; }
        public DateTimeOffset ActionTookPlaceDate { get; set; }
    }
}
