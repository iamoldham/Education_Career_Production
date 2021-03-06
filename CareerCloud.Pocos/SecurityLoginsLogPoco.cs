using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Security_Logins_Log")]
    public class SecurityLoginsLogPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Login { get; set; }

        [Column("Source_IP")]
        [Required]
        public string SourceIP { get; set; }

        [Column("Logon_Date")]
        [Required]
        public DateTime LogonDate { get; set; }

        [Column("Is_Succesful")]
        [Required]
        public bool IsSuccesful { get; set; }


    }
}
