using project.api.Data.DTO.Users;
using project.api.Data.Models;
using System.Collections.Generic;

namespace project.api.Core.Interfaces
{
    public interface IUserServices
    {
        List<User> GetUsers();
        User GetUser(int id);
        User CreateUser(User user);
        AuthenticatedUser SignUp(User user);
        AuthenticatedUser SignIn(User user);

    }
}
