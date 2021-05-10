using Nimb3s.Automaton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql.InMemory
{
    public class WorkItemStatusInMemoryRepository : InMemoryRepository<WorkItemStatusDetailsEntity, Guid>
    {
        private readonly List<WorkItemStatusDetailsEntity> store = new List<WorkItemStatusDetailsEntity>
        {
            new WorkItemStatusDetailsEntity
            {
                Id = Guid.Parse("DB4083F2-7907-47CF-9689-8538CE3A60B1"),
                JobId = Guid.Parse("849B0D53-91BA-432C-BA77-B24105E1F05B"),
                WorkItemStatusTypeId = 4
            },
            new WorkItemStatusDetailsEntity
            {
                Id = Guid.Parse("F3D62A88-FFF5-4EF9-B488-6C8F6F70F68F"),
                JobId = Guid.Parse("78EC60DA-96B0-499A-95D0-E59F81E018F8"),
                WorkItemStatusTypeId = 4
            },
            new WorkItemStatusDetailsEntity
            {
                Id = Guid.Parse("B3592F45-0D8C-4B0E-872C-CBD8C13ABFDA"),
                JobId = Guid.Parse("30694F27-0826-4C7C-AD4E-8E308EDE28FC"),
                WorkItemStatusTypeId = 4
            }
        };

        public async Task<WorkItemStatusDetailsEntity> Get(Guid id)
        {
            return await Task.FromResult(store.FirstOrDefault(i => i.Id == id));
        }
    }
}
