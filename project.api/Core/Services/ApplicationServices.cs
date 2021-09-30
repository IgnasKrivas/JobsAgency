using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.api.Core.Services
{
    public class ApplicationServices : IApplicationServices
    {
        private readonly AppDBContext _context;
        public ApplicationServices(AppDBContext context)
        {
            _context = context;
        }

        public Application CreateApplication(Application application)
        {
            _context.Add(application);
            _context.SaveChanges();

            return application;
        }

        public void DeleteApplication(Application application)
        {
            _context.Applications.Remove(application);
            _context.SaveChanges();
        }

        public void EditApplication(Application application)
        {
            _context.Applications.Update(application);
            _context.SaveChanges();
        }

        public Application GetApplication(int id)
        {
            return _context.Applications.Where(e => e.ApplicationId == id)
                                        .SingleOrDefault();
        }

        public List<Application> GetApplications()
        {
            return _context.Applications.ToList();
        }
        public List<Application> GetApplicationsFromJob(int jobId)
        {
            return _context.Applications.Where(x => x.Job.JobId == jobId).ToList();
        }
        public List<Application> GetApplicationsFromUser(int userId)
        {
            return _context.Applications.Where(x => x.Candidate.Id == userId).ToList();
        }
    }
}
