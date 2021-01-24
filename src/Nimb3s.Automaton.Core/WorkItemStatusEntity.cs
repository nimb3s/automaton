using Nimb3s.Data.Abstractions;
using System;

namespace Nimb3s.Automaton.Core.Entities
{
    public class WorkItemStatusEntity : IEntity<long>
    {
        public long Id { get; set; }
        public Guid WorkItemId { get; set; }
        public short WorkItemStatusTypeId { get; set; }
        public DateTimeOffset StatusTimeStamp { get; set; }
    }
}
