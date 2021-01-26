using Dapper;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class HttpRequestRepository : Repository<HttpRequestEntity, Guid>
    {
        public override string Schema => "[Http]";

        public HttpRequestRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public async Task<IEnumerable<HttpRequestEntity>> GetAllByJobIdAndStatusAsync(Guid jobId, short workItemStatusTypeId)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(jobId), jobId);
            dp.Add(nameof(workItemStatusTypeId), workItemStatusTypeId);

            return (await connection
                .QueryAsync<HttpRequestEntity>(sql: $"{Schema}.p_GetAll{entityName}sByJobIdAndStatus", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                )
                .AsList();
        }
    }
}
