using Microsoft.AspNetCore.Http;
using project.api.Core.CustomExceptions;
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
        private readonly User _user;
        public JobServices(AppDBContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                        .First(u => u.UserName == httpContextAccessor.HttpContext.User.Identity.Name);
        }


        public Job CreateJob(Job job)
        {
            job.UserId = _user.Id;
            _context.Add(job);
            _context.SaveChanges();

            return job;
        }

        public bool DeleteJob(Job job)
        {

            var dbJob = _context.Jobs.Where(j => j.UserId == _user.Id && j.JobId == job.JobId).FirstOrDefault();

            if (dbJob != null)
            {
                _context.Jobs.Remove(dbJob);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool EditJob(Job job)
        {

            var dbJob = _context.Jobs.Where(j => j.UserId == _user.Id && j.JobId == job.JobId).FirstOrDefault();
            if (dbJob != null)
            {
                _context.Jobs.Update(dbJob);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public Job GetJob(int id)
        {
            return _context.Jobs.Where(x => x.JobId == id)
                                .SingleOrDefault();
        }

        public List<Job> GetJobsFromUser(string userId)
        {
            return _context.Jobs.Where(x => x.User.Id == userId).ToList();
        }
        public List<Job> GetJobs()
        {
            return _context.Jobs.ToList();
        }

       
    }
}
