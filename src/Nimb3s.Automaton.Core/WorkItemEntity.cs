using Nimb3s.Data.Abstractions;
using System;

namespace Nimb3s.Automaton.Core.Entities
{
    public class WorkItemEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public DateTimeOffset InsertTimeStamp { get; set; }
    }
}
