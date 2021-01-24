using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class JobRepository : Repository<JobEntity, Guid>
    {
        public override string Schema => "[Job]";

        public JobRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
