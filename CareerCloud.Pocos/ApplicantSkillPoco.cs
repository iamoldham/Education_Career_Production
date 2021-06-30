using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Applicant_Skills")]
    public class ApplicantSkillPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Applicant { get; set; }

        [Required]
        public string Skill { get; set; }

        [Column("Skill_Level")]
        [Required]
        public string SkillLevel { get; set; }

        [Column("Start_Month")]
        [Required]
        public byte StartMonth { get; set; }

        [Column ("Start_Year")]
        [Required]
        public int StartYear { get; set; }

        [Column("End_Month")]
        [Required]
        public byte EndMonth { get; set; }

        [Column("End_Year")]
        [Required]
        public int EndYear { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        [Required]
        public byte[] TimeStamp { get; set; }


    }
}
