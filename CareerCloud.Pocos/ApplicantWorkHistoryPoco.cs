using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Applicant_Work_History")]
    public class ApplicantWorkHistoryPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Applicant { get; set; }

        [Column("Company_Name")]
        [Required]
        public string CompanyName { get; set; }

        [Column("Country_Code")]
        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string Location { get; set; }

        [Column("Job_Title")]
        [Required]
        public string JobTitle { get; set; }

        [Column("Job_Description")]
        [Required]
        public string JobDescription { get; set; }

        [Column("Start_Month")]
        [Required]
        public short StartMonth { get; set; }

        [Column("Start_Year")]
        [Required]
        public int StartYear { get; set; }

        [Column("End_Month")]
        [Required]
        public short EndMonth { get; set; }

        [Column("End_Year")]
        [Required]
        public int EndYear { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        [Required]
        public byte[] TimeStamp { get; set; }

        

    }

}
