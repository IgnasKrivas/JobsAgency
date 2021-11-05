using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Core.Interfaces
{
    public interface IJobServices
    {
        List<Job> GetJobsFromUser(int userId);
        List<Job> GetJobs();
        Job GetJob(int id);
        Job CreateJob(Job job);
        bool EditJob(Job job);
        bool DeleteJob(Job job);

    }
}
