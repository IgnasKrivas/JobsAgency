using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Core.Interfaces
{
    public interface IUserServices
    {
        List<User> GetUsers();
        User GetUser(int id);
        User CreateUser(User user);

    }
}
