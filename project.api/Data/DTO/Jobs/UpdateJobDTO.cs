using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Data.DTO.Jobs
{
    public class UpdateJobDTO
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Requirements { get; set; }
        public double Salary { get; set; }
    }
}
