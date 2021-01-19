﻿using Nimb3s.Automaton.Core.Entities;
using Nimb3s.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Nimb3s.Automaton.Core.Repositories
{
    public class JobStatusRepository : Repository<JobStatusEntity, long>
    {
        public override string Schema => "[Job]";

        public JobStatusRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
    }
}
