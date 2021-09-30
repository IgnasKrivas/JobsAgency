using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Core.Interfaces
{
    public interface IJobServices
    {
        List<Job> GetJobs(int userId);
        Job GetJob(int userId, int id);
        Job CreateJob(Job job);
        void EditJob(Job job);
        void DeleteJob(Job job);

    }
}
