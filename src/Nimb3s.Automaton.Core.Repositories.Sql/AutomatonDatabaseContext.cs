using Nimb3s.Data.Abstractions;
using System.Data.SqlClient;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class AutomatonDatabaseContext : DbContext
    {
        private HttpRequestRepository httpRequestRepository;
        private HttpRequestStatusRepository httpRequestStatusRepository;
        private HttpResponseRepository httpResponseRepository;
        private JobRepository jobRepository;
        private JobStatusRepository jobStatusRepository;

        private WorkItemRepository workItemRepository;
        private WorkItemStatusRepository workItemStatusRepository;

        public HttpRequestRepository HttpRequestRepository =>
                httpRequestRepository ?? (httpRequestRepository = new HttpRequestRepository(UnitOfWork));
        public HttpRequestStatusRepository HttpRequestStatusRepository =>
           httpRequestStatusRepository ?? (httpRequestStatusRepository = new HttpRequestStatusRepository(UnitOfWork));
        public HttpResponseRepository HttpResponseRepository =>
            httpResponseRepository ?? (httpResponseRepository = new HttpResponseRepository(UnitOfWork));
        public JobRepository JobRepository =>
            jobRepository ?? (jobRepository = new JobRepository(UnitOfWork));
        public JobStatusRepository JobStatusRepository =>
            jobStatusRepository ?? (jobStatusRepository = new JobStatusRepository(UnitOfWork));
        public WorkItemRepository WorkItemRepository =>
            workItemRepository ?? (workItemRepository = new WorkItemRepository(UnitOfWork));
        public WorkItemStatusRepository WorkItemStatusRepository =>
           workItemStatusRepository ?? (workItemStatusRepository = new WorkItemStatusRepository(UnitOfWork));

        public AutomatonDatabaseContext()
            :base(new UnitOfWorkFactory<SqlConnection>(@"Data Source=.,1433;Initial Catalog=Automaton;User ID=sa;Password=1234qwerASDF;"))
        {

        }


        public AutomatonDatabaseContext(IUnitOfWorkFactory unitOfWorkFactory)
            :base(unitOfWorkFactory)
        {

        }
    }
}
