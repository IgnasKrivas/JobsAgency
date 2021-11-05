using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Data.DTO.Users
{
    public class AuthenticatedUser
    {
        public string Token { get; set; }
        public string Username { get; set; }
    }
}
