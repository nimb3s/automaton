using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nimb3s.Automaton.Core.Repositories
{
    public class WorkItemRepository : Repository<WorkItemEntity>
    {
        public override string Schema => "Job";

        public WorkItemRepository() { }

        public WorkItemRepository(IDbConnection dbConnection) 
            :base(dbConnection) 
        {
        }
    }
}
