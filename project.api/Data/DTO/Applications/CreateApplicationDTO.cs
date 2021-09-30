using System;
using System.Collections.Generic;
using System.Text;

namespace project.api.Data.DTO.Applications
{
    public class CreateApplicationDTO
    {

            public string Summary { get; set; }
            public string Education { get; set; }
            public string Languages { get; set; }
            public string Experience { get; set; }
            public string Course { get; set; }
            public int JobId { get; set; }
            public int CandidateId { get; set; }
    }
}
