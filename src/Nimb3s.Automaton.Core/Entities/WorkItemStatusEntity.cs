using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Entities
{
    public class WorkItemStatusEntity : IEntity<long>
    {
        public long Id { get; set; }
        public Guid WorkItemId { get; set; }
        public short WorkItemStatusId { get; set; }
        public DateTimeOffset StatusTimeStamp { get; set; }
    }
}
