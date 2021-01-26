using Nimb3s.Data.Abstractions;
using System;

namespace Nimb3s.Automaton.Core.Entities
{
    public class JobEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string JobName { get; set; }
        public DateTimeOffset InsertTimeStamp { get; set; }
    }
}
