using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace project.api.Data.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }  
        public string Name { get; set; }  
        public string Summary { get; set; }  
        public string Location { get; set; }  
        public string Requirements { get; set; }  
        public double Salary { get; set; }
        [JsonIgnore]
        public ICollection<Application> Applications { get; set; }
        //[ForeignKey("UserId")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
