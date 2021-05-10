using Nimb3s.Automaton.Core.Repositories.Sql.InMemory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class AutomatonDatabaseInMemoryContext : IAutomatonInMemoryContext
    {
        private JobStatusInMemoryRepository jobStatusInMemoryRepository;
        private HttpRequestStatusInMemoryRepository httpRequestStatusInMemoryRepository;
        private WorkItemStatusInMemoryRepository workItemStatusInMemoryRepository;

        public JobStatusInMemoryRepository JobStatusInMemoryRepository => 
            jobStatusInMemoryRepository ?? (jobStatusInMemoryRepository = new JobStatusInMemoryRepository());

        public HttpRequestStatusInMemoryRepository HttpRequestStatusInMemoryRepository => 
            httpRequestStatusInMemoryRepository ?? (httpRequestStatusInMemoryRepository = new HttpRequestStatusInMemoryRepository());

        public WorkItemStatusInMemoryRepository WorkItemStatusInMemoryRepository => 
            workItemStatusInMemoryRepository ?? (workItemStatusInMemoryRepository = new WorkItemStatusInMemoryRepository());
    }
}
