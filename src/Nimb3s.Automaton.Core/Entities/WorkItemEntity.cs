using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Entities
{
    public class WorkItemEntity : IEntity<Guid>, IDisposable
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public short WorkItemStatusId { get; set; }

        public void Dispose()
        {

        }
    }
}
