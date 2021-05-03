using Dapper;
using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Automaton.Core.Repositories.Sql.InMemory;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class JobStatusInMemoryRepository : InMemoryRepository<JobStatusDetailsEntity, Guid>
    {
        private readonly List<JobStatusDetailsEntity> store = new List<JobStatusDetailsEntity>
        {
            new JobStatusDetailsEntity
            {
                Id = Guid.Parse("849B0D53-91BA-432C-BA77-B24105E1F05B"),
                JobId = Guid.Parse("849B0D53-91BA-432C-BA77-B24105E1F05B"),
                JobName = "Test Job 1",
                JobStatusTypeId = 1
            },
            new JobStatusDetailsEntity
            {
                Id = Guid.Parse("E424A391-1FE8-4980-8BFE-11417E4B5412"),
                JobId = Guid.Parse("793C2D1F-5F96-4EC7-B417-340FFAF58E62"),
                JobName = "Test Job 2",
                JobStatusTypeId = 1
            },
            new JobStatusDetailsEntity
            {
                Id = Guid.Parse("98FB4847-0032-46B9-89B2-697006E6DA8B"),
                JobId = Guid.Parse("529018AB-4E0A-47D6-BEE4-E00CA999F5FB"),
                JobName = "Test Job 3",
                JobStatusTypeId = 1
            }
        };

        public async Task<JobStatusDetailsEntity> Get(Guid id)
        {
            return await Task.FromResult(store.FirstOrDefault(i => i.Id == id));
        }

    }

}
