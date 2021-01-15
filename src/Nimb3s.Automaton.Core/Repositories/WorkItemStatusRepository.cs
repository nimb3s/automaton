using Dapper;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories
{
    public class WorkItemStatusRepository : Repository<WorkItemStatusEntity, long>
    {
        public override string Schema => "[Job]";

        public WorkItemStatusRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public async Task<WorkItemStatusEntity> GetByWorkItemIdAsync(Guid workItemId)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(workItemId), workItemId);

            return await connection
                .QuerySingleAsync<WorkItemStatusEntity>(sql: $"{Schema}.p_Get{entityName}By{nameof(workItemId)}", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                ;
        }
    }
}
