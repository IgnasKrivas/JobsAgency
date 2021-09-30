using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Core.Interfaces
{
    public interface IApplicationServices
    {
        List<Application> GetApplications(int jobId);
        Application GetApplication(int jobId,int id);
        Application CreateApplication(Application application);
        void EditApplication(Application application);
        void DeleteApplication(Application application);

    }
}
