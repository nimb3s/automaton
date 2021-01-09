using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Nimb3s.Automaton.Core
{
    public class AutomatonDatabaseContext : DbContext
    {
        private HttpRequestRepository httpRequestRepository;
        private HttpResponseRepository httpResponseRepository;
        private JobRepository jobRepository;
        private WorkItemRepository workItemRepository;

        public HttpRequestRepository HttpRequestRepository =>
                httpRequestRepository ?? (httpRequestRepository = new HttpRequestRepository(UnitOfWork));
        public HttpResponseRepository HttpResponseRepository =>
            httpResponseRepository ?? (httpResponseRepository = new HttpResponseRepository(UnitOfWork));
        public JobRepository JobRepository =>
            jobRepository ?? (jobRepository = new JobRepository(UnitOfWork));
        public WorkItemRepository WorkItemRepository =>
            workItemRepository ?? (workItemRepository = new WorkItemRepository(UnitOfWork));

        public AutomatonDatabaseContext()
            :base(new UnitOfWorkFactory<SqlConnection>(@"Data Source=.\sqlexpress;Initial Catalog=Automaton;Integrated Security=true"))
        {

        }


        public AutomatonDatabaseContext(IUnitOfWorkFactory unitOfWorkFactory)
            :base(unitOfWorkFactory)
        {

        }
    }
}
