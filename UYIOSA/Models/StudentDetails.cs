using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UYIOSA.Models
{
    public class StudentDetails
    {
        [Key]
        public int StudentId { get; set; }

        //automatically generated from login Details
        [DisplayName("School")]
        public string StudentSchoolName { get; set; }

        //Profile
        [Required]
        [DisplayName("First Name")]
        public string StudentFirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string StudentLastName { get; set; }

        [Required]
        [DisplayName("Mat No")]
        public string StudentMatNo { get; set; }

        [Required]
        [DisplayName("Phone No")]
        public string StudentPhoneNo { get; set; }

        //available only in batabase
        [Required]
        [DisplayName("Gender")]
        public Gender StudentSex { get; set; }

        //Login Details
      //  [Required]
       [DisplayName("User Name")]
       public string StudentUserName { get; set; }

      //[Required]
      [DisplayName("Password")]
       [DataType(DataType.Password)]
       public string StudentPassword { get; set; }

        //checks
        [DisplayName("Status")]
        public string StudentStatus { get; set; }

        public string StudentRegisteredTime { get; set; }

        [DisplayName("ID Status")]
        public string StudentIdCardStatus { get; set; }

        [DisplayName("Accomodation")]
        public string StudentSpace { get; set; }

        public byte[] Photo { get; set; }

        //for interaction through dropdown
        // [Required]
        // [DisplayName("Gender")]
        // public Gender StudentGender { get; set; }

        public virtual ICollection<File> Files { get; set; }

    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
        Others = 3
    }
}