using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserFinishedQueueingJobMessage: IMessage
    {
        public Guid JobId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}
