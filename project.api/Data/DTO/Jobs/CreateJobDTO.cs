using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Data.DTO.Jobs
{
    public class CreateJobDTO
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Requirements { get; set; }
        public double Salary { get; set; }
    }
}
