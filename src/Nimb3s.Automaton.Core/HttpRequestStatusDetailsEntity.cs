using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Entities
{
    public class HttpRequestStatusDetailsEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public short HttpRequestStatusTypeId { get; set; }
        public string Url { get; set; }
        public short StatusCode { get; set; }
        public string Body { get; set; }
    }
}
