using NServiceBus;
using System;
using System.Collections.Generic;

namespace Nimb3s.Automaton.Messages
{
    public class UserSubmittedHttpRequestMessage : IMessage
    {
        public HttpRequest HttpRequest { get; set; }
    }
}
