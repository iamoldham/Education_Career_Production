using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Company_Jobs")]
    public class CompanyJobPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Company { get; set; }

        [Column("Profile_Created")]
        [Required]
        public DateTime ProfileCreated { get; set; }

        [Column("Is_Inactive")]
        [Required]
        public bool IsInactive { get; set; }

        [Column("Is_Company_Hidden")]
        [Required]
        public bool IsCompanyHidden { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        public byte[] TimeStamp { get; set; }

        
    }

}
