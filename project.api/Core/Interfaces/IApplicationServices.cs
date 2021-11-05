using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Core.Interfaces
{
    public interface IApplicationServices
    {
        //List<Application> GetApplicationsFromUser(int userId);
        List<Application> GetApplicationsFromJob(int jobId);
        List<Application> GetApplications();
        Application GetApplication(int id);
        Application CreateApplication(Application application);
        bool EditApplication(Application application);
        bool DeleteApplication(Application application);

    }
}
