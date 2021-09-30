using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.api.Core.Services
{
    public class JobServices : IJobServices
    {
        private readonly AppDBContext _context;
        public JobServices(AppDBContext context)
        {
            _context = context;
        }

        public Job CreateJob(Job job)
        {
            _context.Add(job);
            _context.SaveChanges();

            return job;
        }

        public void DeleteJob(Job job)
        {
            _context.Jobs.Remove(job);
            _context.SaveChanges();
        }

        public void EditJob(Job job)
        {
            _context.Jobs.Update(job);
            _context.SaveChanges();
        }

        public Job GetJob(int id)
        {
            return _context.Jobs.Where(x => x.JobId == id)
                                .SingleOrDefault();
        }

        public List<Job> GetJobsFromUser(int userId)
        {
            return _context.Jobs.Where(x => x.User.Id == userId).ToList();
        }
        public List<Job> GetJobs()
        {
            return _context.Jobs.ToList();
        }
    }
}
