using Dapper;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class JobStatusRepository : Repository<JobStatusEntity, long>
    {
        public override string Schema => "[Job]";

        public JobStatusRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public async Task<JobStatusDetailsEntity> GetByJobStatusIdAsync(Guid jobId)
        { 
            DynamicParameters dp = new DynamicParameters();

            dp.Add(nameof(jobId), jobId);

            return await connection
                .QuerySingleAsync<JobStatusDetailsEntity>(sql: $"{Schema}.p_Get{entityName}DetailsBy{nameof(jobId)}", param: dp, commandType: CommandType.StoredProcedure, transaction: transaction)
                .ConfigureAwait(false);
        }
    }
}
