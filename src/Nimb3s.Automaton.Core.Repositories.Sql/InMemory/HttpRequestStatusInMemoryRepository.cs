using Nimb3s.Automaton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Core.Repositories.Sql.InMemory
{
    public class HttpRequestStatusInMemoryRepository : InMemoryRepository<HttpRequestStatusDetailsEntity, Guid>
    {
        private readonly List<HttpRequestStatusDetailsEntity> store = new List<HttpRequestStatusDetailsEntity>
        {
            new HttpRequestStatusDetailsEntity
            {
                Id = Guid.Parse("03C8D543-DBDB-4A1F-98A4-87FFF1D8CE9C"),
                HttpRequestStatusTypeId = 3,
                Url = "https://www.cnn.com/",
                StatusCode = 200,
                Body = "HTML test"
            },
            new HttpRequestStatusDetailsEntity
            {
                Id = Guid.Parse("F9C3CC1F-945E-4911-857E-7ED69F8796E3"),
                HttpRequestStatusTypeId = 3,
                Url = "https://www.fox.com/",
                StatusCode = 200,
                Body = "HTML test 2"
            },
            new HttpRequestStatusDetailsEntity
            {
                Id = Guid.Parse("876A3B95-07BC-4852-BEC5-96CC8AF74E3F"),
                HttpRequestStatusTypeId = 3,
                Url = "https://www.yahoo.com/",
                StatusCode = 200,
                Body = "HTML test 3"
            }
        };

        public async Task<HttpRequestStatusDetailsEntity> Get(Guid id)
        {
            return await Task.FromResult(store.FirstOrDefault(i => i.Id == id));
        }
    }
}
