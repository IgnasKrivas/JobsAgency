using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace project.api.Data.Models
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }


        //[ForeignKey("ApplicationId")]
        public int ApplicationId { get; set; }
        [JsonIgnore]
        public Application Application { get; set; }
    }
}
