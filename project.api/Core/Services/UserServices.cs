using Microsoft.AspNet.Identity;
//using Microsoft.AspNetCore.Identity;
using project.api.Core.CustomExceptions;
using project.api.Core.Interfaces;
using project.api.Core.Utilities;
using project.api.Data.Context;
using project.api.Data.DTO.Users;
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
        private readonly IPasswordHasher _passwordHasher;

        public UserServices(AppDBContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;

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

        public AuthenticatedUser SignIn(User user)
        {
            var dbUser = _context.Users
                .FirstOrDefault(u => u.Username == user.Username);
            if(dbUser == null
                || _passwordHasher.VerifyHashedPassword(dbUser.Password,user.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid username or password");
            }
            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username)
            };
        }

        public AuthenticatedUser SignUp(User user)
        {
            var checkUser = _context.Users.
                FirstOrDefault(u => u.Username.Equals(user.Username));
            if(checkUser != null)
            {
                throw new UsernameAlreadyExistsException("Username already exists");
            }
            user.Password = _passwordHasher.HashPassword(user.Password);
            _context.Add(user);
            _context.SaveChanges();
            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JwtGenerator.GenerateUserToken(user.Username)
            };
        }
    }
}
