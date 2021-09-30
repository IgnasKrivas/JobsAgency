using project.api.Core.Interfaces;
using project.api.Data.Context;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project.api.Core.Services
{
    public class UserServices : IUserServices
    {
        private readonly AppDBContext _context;
        public UserServices(AppDBContext context)
        {
            _context = context;
        }

        public User CreateUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetUser(int id)
        {
            return _context.Users.First(e => e.Id == id);
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}
