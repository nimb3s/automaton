using Dapper;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class HttpRequestStatusRepository : Repository<HttpRequestStatusEntity, long>
    {
        public override string Schema => "[Http]";

        public HttpRequestStatusRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        public async Task<HttpRequestStatusDetailsEntity> GetByHttpRequestIdAsync(Guid HttpRequestId)
        {
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(HttpRequestId), HttpRequestId);

            return await connection
                .QuerySingleAsync<HttpRequestStatusDetailsEntity>(sql: $"{Schema}.p_Get{entityName}By{nameof(HttpRequestId)}", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                .ConfigureAwait(false);
        }
    }
}
