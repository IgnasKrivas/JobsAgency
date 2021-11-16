using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace project.api.Data.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }
        public string Languages { get; set; }  
        public string Experience { get; set; }  
        public string Course { get; set; }
        [JsonIgnore]
        public ICollection<Skill> Skills { get; set; }
        //[ForeignKey("JobId")]

        public int JobId { get; set; }
        [JsonIgnore]
        public Job Job { get; set; }
        //[ForeignKey("UserId")]
        [Required]
        public string CandidateId { get; set; }
        [JsonIgnore]
        public User Candidate { get; set; }


        // User Id
        //  Offering Id
    }
}
