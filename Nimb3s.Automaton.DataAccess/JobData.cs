using Nimb3s.Automaton.Pocos;
using Nimb3s.Automaton.Pocos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimb3s.Automaton.DataAccess
{
    public class JobData
    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();

        public JobData()
        {
            _connectionString = "Data Source=AAVELDANEZ-PC;Initial Catalog=Automaton;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public JobStatusModel GetJobStatusById(Guid jobId)
        {
            string sql = @"SELECT j.JobName, jst.Enumeration
                            FROM Job.Job j
                            INNER JOIN Job.JobStatus js
                            ON j.Id = js.JobId
                            INNER JOIN Job.JobStatusType jst
                            ON js.JobStatusTypeId = jst.Id
                            WHERE js.JobId = @JobId";


            JobStatusModel job = new JobStatusModel();

            job = db.GetData<JobStatusModel, dynamic>(sql, new { JobId = jobId }, _connectionString).FirstOrDefault();

            return job;
        }

    }
}
