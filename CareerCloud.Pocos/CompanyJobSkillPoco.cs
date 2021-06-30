using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Company_Job_Skills")]
    public class CompanyJobSkillPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Job { get; set; }

        [Column("Skill")]
        [Required]
        public string Skill { get; set; }

        [Column("Skill_Level")]
        [Required]
        public string SkillLevel { get; set; }

        [Required]
        public int Importance { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        [Required]
        public byte[] TimeStamp { get; set; }

        


        

    }

}
