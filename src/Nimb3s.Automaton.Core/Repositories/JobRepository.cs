using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nimb3s.Automaton.Core.Repositories
{
    public class JobRepository : Repository<JobEntity>
    {
        public override string Schema => "Job";

        public JobRepository() { }

        public JobRepository(IDbConnection dbConnection) 
            :base(dbConnection) 
        {
        }
    }
}
