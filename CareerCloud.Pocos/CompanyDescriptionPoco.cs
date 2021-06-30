using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Company_Descriptions")]
    public class CompanyDescriptionPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Company { get; set; }

        [Required]
        public string LanguageId { get; set; }

        [Column("Company_Name")]
        [Required]
        public string CompanyName { get; set; }

        [Column("Company_Description")]
        [Required]
        public string CompanyDescription { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        [Required]
        public byte[] TimeStamp { get; set; }

       
    }
}
