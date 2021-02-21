using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Entities
{
    public class JobStatusDetailsEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public short JobStatusTypeId { get; set; }
    }
}
