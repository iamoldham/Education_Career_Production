using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CareerCloud.Pocos
{
    [Table("Applicant_Educations")]
    public class ApplicantEducationPoco : IPoco
    {

        [Key]
        [Required]
        public Guid Id { get; set; }

        //Setting foreign key
        [Required]
        [ForeignKey("ApplicantProfilePoco")]
        public Guid Applicant { get; set; } //foreign key needs to be set Ap-profiles-id
        public ApplicantProfilePoco ApplicantProfilePoco { get; set; }


        [Required]
        public string Major { get; set; }

        [Column("Certificate_Diploma")]
        public string CertificateDiploma { get; set; }

        [Column("Start_Date")]
        public DateTime? StartDate { get; set; }

        [Column("Completion_Date")]
        public DateTime? CompletionDate { get; set; }

        [Column("Completion_Percent")]
        public byte? CompletionPercent { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        
        
    }
}
