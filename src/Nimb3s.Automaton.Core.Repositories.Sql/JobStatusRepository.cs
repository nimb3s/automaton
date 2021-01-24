using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class JobStatusRepository : Repository<JobStatusEntity, long>
    {
        public override string Schema => "[Job]";

        public JobStatusRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
