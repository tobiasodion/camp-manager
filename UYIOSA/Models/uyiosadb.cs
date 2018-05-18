using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UYIOSA.Models
{
    public class uyiosadb : DbContext
    {
        public uyiosadb() : base("name=DefaultConnection")
        {

        }
        public DbSet<AdminDetails> Admin { get; set; }
        public DbSet<SchoolDetails> School { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<StudentDetails> Student { get; set; }
    }
}

/* for (int i= 0; i < 1000; i++) {

context.School.AddOrUpdate(
                  p => p.SchoolName,
                  new SchoolDetails { SchoolName = i.ToString(), NumberOfStudents = 20, RegistrationAmount = "unknown", SchoolCoordinatorMatNo = "unknown", SchoolCoordinatorPhoneNo = "unknown", SchoolLocation = "unknown", SchoolRegisteredBy = "unknown", SchoolRegisteredTime = "unknown", SchoolRegistrationStatus = "unknown", }

                );
            }*/