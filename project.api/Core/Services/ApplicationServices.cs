using Microsoft.AspNetCore.Http;
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
        private readonly User _user;
        public ApplicationServices(AppDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _user = _context.Users
                        .First(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public Application CreateApplication(Application application)
        {
            
            application.CandidateId = _user.Id;
            _context.Add(application);
            _context.SaveChanges();

            return application;
        }

        public bool DeleteApplication(Application application)
        {
            var dbApp = _context.Applications.Where(a => a.CandidateId == _user.Id && a.ApplicationId == application.ApplicationId).FirstOrDefault();
            if (dbApp != null) { 
            _context.Applications.Remove(application);
            _context.SaveChanges();
                return true;
        }
        else{
                return false;
        }
        }

        public bool EditApplication(Application application)
        {
            var dbApp = _context.Applications.Where(a => a.CandidateId == _user.Id && a.ApplicationId == application.ApplicationId).FirstOrDefault();
            if (dbApp != null)
            {
                _context.Applications.Update(application);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Application GetApplication(int id)
        {
            return _context.Applications.Where(e => e.ApplicationId == id)
                                        .SingleOrDefault();
        }

        public List<Application> GetApplications()
        {
            return _context.Applications.Where(a => a.CandidateId == _user.Id).ToList();
        }
        public List<Application> GetApplicationsFromJob(int jobId)
        {
            return _context.Applications.Where(x => x.Job.JobId == jobId).ToList();
        }
        //public List<Application> GetApplicationsFromUser(int userId)
        //{
        //    return _context.Applications.Where(x => x.Candidate.Id == userId).ToList();
        //}
    }
}
