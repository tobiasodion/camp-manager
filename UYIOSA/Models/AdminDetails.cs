using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UYIOSA.Models
{
    public class AdminDetails
    {
        [Key]
        public int Id { get; set; }

       
        [Required]
        [DisplayName("Mat No")]
        public string MatNo { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Re-type Password")]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }

        public string Status { get; set; }

        public string Role { get; set; }

        public string AdminRegistrationTime { get; set; }


    }
}