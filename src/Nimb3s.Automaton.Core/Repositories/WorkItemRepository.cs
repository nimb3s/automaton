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
    public class WorkItemRepository : Repository<WorkItemEntity, Guid>
    {
        public override string Schema => "[Job]";

        public WorkItemRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public async Task<IEnumerable<WorkItemEntity>> GetAllByJobIdAsync(Guid jobId)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(jobId), jobId);

            return (await connection
                .QueryAsync<WorkItemEntity>(sql: $"{Schema}.p_GetAll{entityName}sByJobId", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                .ConfigureAwait(false))
                .AsList();

        }
    }
}
