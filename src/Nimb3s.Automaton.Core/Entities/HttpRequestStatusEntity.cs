using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Entities
{
    public class HttpRequestStatusEntity : IEntity<long>
    {
        public long Id { get; set; }
        public Guid HttpRequestId { get; set; }
        public short HttpRequestStatusTypeId { get; set; }
        public DateTimeOffset StatusTimeStamp { get; set; }
    }
}
