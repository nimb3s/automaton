using Nimb3s.Automaton.Pocos;
using NServiceBus;
using System;

namespace Nimb3s.Automaton.Messages.User
{
    public class UserSubmittedHttpRequestMessage : IMessage
    {
        public Guid JobId { get; set; }
        public Guid WorkItemId { get; set; }
        public UserHttpRequest HttpRequest { get; set; }
    }

   
}
