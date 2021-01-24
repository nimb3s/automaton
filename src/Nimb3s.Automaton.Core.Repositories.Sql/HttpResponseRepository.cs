using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;

namespace Nimb3s.Automaton.Core.Repositories.Sql
{
    public class HttpResponseRepository : Repository<HttpResponseEntity, Guid>
    {
        public override string Schema => "[Http]";

        public HttpResponseRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
