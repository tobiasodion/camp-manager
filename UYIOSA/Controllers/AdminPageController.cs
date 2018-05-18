using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UYIOSA.Models;

namespace UYIOSA.Controllers
{
    public class AdminPageController : Controller
    {
        uyiosadb db = new uyiosadb();

        // GET: AdminPage
        public ActionResult Index()
        {
            return View();
        }

        //Managing school
        public ActionResult RegisterSchool()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterSchool(SchoolDetails details)
        {

            if (ModelState.IsValid)
            {
               try
                {
                   // String test1 = Session["MatNo"].ToString();
                    String test = Session["FirstName"].ToString();
                    String test1 = Session["LastName"].ToString();

                    details.RegistrationAmount = details.NumberOfStudents * 1500;
                    details.SchoolRegistrationStatus = "In Progress";
                    details.SchoolRegisteredBy = test1 + " " + test;
                    db.School.Add(details);

                    String SchoolName = details.SchoolName;
                    // int count = details.NumberOfStudents;

                    ViewBag.Message = "FECAMDS " + details.SchoolName + " " + "Successfully Registered";




                   // for (int n = 0; n < details.NumberOfStudents; n++)
                    //{

                       // StudentDetails model = new StudentDetails();

                       // model.StudentSchoolName = SchoolName;
                       // model.StudentUserName = "Tobias";
                       // model.StudentPassword = "BabaHelpMe";
                       // model.StudentFirstName = "Pending";
                       // model.StudentLastName = "Pending";
                       // model.StudentMatNo = "Pending";
                       // model.StudentPhoneNo = "Pending";
                       // model.StudentSex = "Pending";
                        //db.Student.Add(model);
                   // }

                    db.SaveChanges();
                    ModelState.Clear();
                    return View();
                }

                catch (Exception ex)
                {

                    throw;
                }
            }
            return View();
        }

        public ActionResult ManageSchool()
        {
            var model = (from r in db.School
                         select r).ToList();
            return View(model);
        }

        public ActionResult DownloadActionAsPDF(int id)
        {  
            try
            {
                var query = (from r in db.School
                             where r.SchoolId == id
                             select r).SingleOrDefault();

                string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

                String PdfName = query.SchoolName.ToString() + "_Receipt.pdf";
                //will take ActionMethod and generate the pdf
                return new Rotativa.ActionAsPdf("GeneratePDF", new { mid = id }) { FileName = PdfName, CustomSwitches = footer};
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult GeneratePDF(int mid)
        {
             try
             {
            var model = new GeneratePDFModel();

            //Your data from db
            var query = (from r in db.School
                         where r.SchoolId == mid
                         select r).SingleOrDefault();

            //hard coded value for test purpose
            String Name = query.SchoolName.ToString();
            String Location = query.SchoolLocation.ToString();
            String RegisteringAdmin = query.SchoolRegisteredBy.ToString();
            String Amount = query.RegistrationAmount.ToString();
            String NumberOfStudents = query.NumberOfStudents.ToString();


            var content = "NAME: " + "FECAMDS " + Name + "<br>" + "<br>" +
                          "LOCATION: " + Location + "<br>" + "<br>" +
                          "NUMBER OF STUDENTS: " + NumberOfStudents + "<br>" + "<br>" +
                          "AMOUNT: " + "#" + Amount + "<br>" + "<br>" +
                          "REGISTERED BY : " + RegisteringAdmin;

            var logoFile = @"/Images/logo.png";

            model.PDFContent = content;
            model.PDFLogo = Server.MapPath(logoFile);

            return View(model);
        }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult GenerateStudentLoginSlip(int id)
        {
            try
            {
                var query = (from r in db.School
                             where r.SchoolId == id
                             select r).SingleOrDefault();

                

                string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

                String PdfName = query.SchoolName.ToString() + "Student_Login_Slip.pdf";
                //will take ActionMethod and generate the pdf
                return new Rotativa.ActionAsPdf("GeneratePDF2", new { mid = id} ) { FileName = PdfName, CustomSwitches = footer };
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public ActionResult GeneratePDF2(int mid)
        {
            try
            {
               // var model = new GeneratePDFModel();

                var query = (from r in db.School
                             where r.SchoolId == mid
                             select r).SingleOrDefault();

                //Your data from db
                String SchoolName = query.SchoolName.ToString();

                var model = (from r in db.Student
                             where r.StudentSchoolName == SchoolName
                             select r).ToList();

                //hard coded value for test purpose
               /* String Name = query.SchoolName.ToString();
                String Location = query.SchoolLocation.ToString();
                String RegisteringAdmin = query.SchoolRegisteredBy.ToString();
                String Amount = query.RegistrationAmount.ToString();
                String NumberOfStudents = query.NumberOfStudents.ToString();


                var content = "NAME: " + "FECAMDS " + Name + "<br>" + "<br>" +
                              "LOCATION: " + Location + "<br>" + "<br>" +
                              "NUMBER OF STUDENTS: " + NumberOfStudents + "<br>" + "<br>" +
                              "AMOUNT: " + "#" + Amount + "<br>" + "<br>" +
                              "REGISTERED BY : " + RegisteringAdmin;

                var logoFile = @"/Images/logo.png";*/

               // model.PDFContent = content;
               // model.PDFLogo = Server.MapPath(logoFile);
                
                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult GenerateStudentIDCard()
        {
            return View();
        }

        //managing Student

        public ActionResult RegisteredStudentsList()
        {
            return View();
        }

        public ActionResult ViewRegisteredStudents()
        {
            return View();
        }

        public ActionResult EditRegisteredStudents()
        {
            return View();
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