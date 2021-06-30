using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CareerCloud.Pocos
{
    [Table("Security_Logins")]
    public class SecurityLoginPoco : IPoco
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Column("Created_Date")]
        [Required]
        public DateTime Created { get; set; }

        [Column("Password_Update_Date")]
        public DateTime? PasswordUpdate { get; set; }

        [Column("Agreement_Accepted_Date")]
        public DateTime? AgreementAccepted { get; set; }

        [Column("Is_Locked")]
        [Required]
        public bool IsLocked { get; set; }

        [Column("Is_Inactive")]
        [Required]
        public bool IsInactive { get; set; }

        [Column("Email_Address")]
        [Required]
        public string EmailAddress { get; set; }

        [Column("Phone_Number")]
        public string PhoneNumber { get; set; }

        [Column("Full_Name")]
        public string FullName { get; set; }

        [Column("Force_Change_Password")]
        [Required]
        public bool ForceChangePassword { get; set; }

        [Column("Prefferred_Language")]
        public string PrefferredLanguage { get; set; }

        [Column("Time_Stamp")]
        [Timestamp]
        [Required]
        public byte[] TimeStamp { get; set; }

   
    }

}
