using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UYIOSA.Models;

namespace UYIOSA.Controllers
{
    public class StudentPageController : Controller
    {
        uyiosadb db = new uyiosadb();

        // GET: Users/Create  
        public ActionResult Register()
        {
            String schoolName = Session["StudentSchoolName"].ToString();
            ViewBag.Greeting = "Hello, " + schoolName + " FECAMDsite";
            return View();
        }

        [HttpPost]
        public ActionResult Register(StudentDetails student, HttpPostedFileBase upload)
        {
            try {

                if (ModelState.IsValid)
                {
                    String User = Session["StudentUserName"].ToString();
                    String Password = Session["StudentPassword"].ToString();


                    var query = (from r in db.Student
                                 where r.StudentUserName == User && r.StudentPassword == Password
                                 select r).SingleOrDefault();


                    if (upload != null && upload.ContentLength > 0)
                    {
                        var avatar = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            avatar.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        query.Files = new List<File> { avatar };

                        query.StudentFirstName = student.StudentFirstName;
                        query.StudentLastName = student.StudentLastName;
                        query.StudentMatNo = student.StudentMatNo;
                        query.StudentPhoneNo = student.StudentPhoneNo; 
                        query.StudentSex = student.StudentSex;
                        query.StudentStatus = "Registered";
                        query.StudentIdCardStatus = "Pending";
                        query.StudentSpace = "Room Unknown";

                        
                        DateTime Now = DateTime.Now;
                        String time = Now.ToString();
                        query.StudentRegisteredTime = time;

                        

                      var model = (from r in db.School
                                     where r.SchoolName == query.StudentSchoolName
                                     select r).SingleOrDefault();

                        model.SchoolProgressCounter++;

                        if (model.SchoolProgressCounter== model.NumberOfStudents)
                        {
                            model.SchoolRegistrationStatus = "Completed";
                        }

                        db.SaveChanges();
                        Session["Successful Registration"] = "Registration Successful";

                        return RedirectToAction("StudentLogin", "StudentAccount");
                    }

                  Session["failed Registration"] = "Registration Unsuccessful!";
                return RedirectToAction("StudentLogout","StudentAccount");
                }
            else
            {
                ModelState.Clear();
                    Session["failed Registration"] = "Registration Unsuccessful!";
                    return RedirectToAction("StudentLogout", "StudentAccount");
                }
       }
           catch (Exception ex)
           {
               throw;
            }
     }
        

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}