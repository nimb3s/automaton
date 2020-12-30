using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nimb3s.Automaton.Core.Repositories
{
    public class HttpRequestRepository : Repository<HttpRequestEntity>
    {
        public override string Schema => "[Http]";

        public HttpRequestRepository() { }

        public HttpRequestRepository(IDbConnection dbConnection) 
            :base(dbConnection) 
        {
        }
    }
}
