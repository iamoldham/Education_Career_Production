using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("System_Language_Codes")]
    public class SystemLanguageCodePoco
    {
        [Key]
        [Required]
        public string LanguageID { get; set; }

        [Required]
        public string Name { get; set; }

        [Column("Native_Name")]
        [Required]
        public string NativeName { get; set; }

      
    }


}
