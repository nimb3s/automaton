using Nimb3s.Automaton.Core.Repositories.Sql.InMemory;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public interface IAutomatonInMemoryContext
    {
        JobStatusInMemoryRepository JobStatusInMemoryRepository { get; }
        HttpRequestStatusInMemoryRepository HttpRequestStatusInMemoryRepository { get; }
        WorkItemStatusInMemoryRepository WorkItemStatusInMemoryRepository { get;  }
    }
}
