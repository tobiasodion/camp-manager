using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UYIOSA.Models
{
    public class SchoolDetails
    {
        [Key]
        public int SchoolId { get; set; }

        [Required]
        [DisplayName("School Name")]
        public String SchoolName { get; set; }

        [Required]
        [DisplayName("Location")]
        public String SchoolLocation { get; set; }

        [Required]
        [DisplayName("Coordinator Mat No")]
        public String SchoolCoordinatorMatNo { get; set; }

        [Required]
        [DisplayName("Coordinator Phone No")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11)]
        public String SchoolCoordinatorPhoneNo { get; set; }

        [Required]
        [DisplayName("Number of Students")]
        public int NumberOfStudents { get; set; }

        [DataType(DataType.Currency)]
        public long RegistrationAmount { get; set; }

        [DisplayName("Registration Status")]
        public String SchoolRegistrationStatus { get; set; }

        public String SchoolRegisteredBy { get; set; }

        public String SchoolRegisteredTime { get; set; }

        public int SchoolProgressCounter { get; set; }


    }
}