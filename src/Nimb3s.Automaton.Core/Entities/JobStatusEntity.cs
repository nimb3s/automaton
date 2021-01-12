using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Entities
{
    public class JobStatusEntity : IEntity<long>
    {
        public long Id { get; set; }
        public Guid JobId { get; set; }
        public short JobStatusId { get; set; }
        public DateTimeOffset StatusTimeStamp { get; set; }
    }
}
