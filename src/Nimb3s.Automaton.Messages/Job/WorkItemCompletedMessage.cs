﻿using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.Job
{
    public class WorkItemCompletedMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public Guid HttpRequestId { get; set; }
        public DateTimeOffset DateActionTaken { get; set; }
    }
}
