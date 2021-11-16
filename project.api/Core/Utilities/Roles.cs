using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Core.Utilities
{
    public static class Roles
    {
        public const string Admin = nameof(Admin);
        public const string SimpleUser = nameof(SimpleUser);
        public const string Employer = nameof(Employer);
        public static readonly IReadOnlyCollection<string> All = new[] { Admin, SimpleUser,Employer };
    }
}
