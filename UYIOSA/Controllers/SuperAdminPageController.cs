using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UYIOSA.Models;
using System.Security.Cryptography;
using PagedList;

namespace UYIOSA.Controllers
{
    public class SuperAdminPageController : Controller
    {
        uyiosadb db = new uyiosadb();
        int receiptAmount;
        int registrationFee = 1500;

        // GET: AdminPage
        public ActionResult Index()
        {
            return View();
        }

        //Managing Schools

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
                    String SchoolName = details.SchoolName;

                for (int n = 0; n < details.NumberOfStudents; n++)
                {

                    StudentDetails model = new StudentDetails();

                    model.StudentSchoolName = SchoolName;
                    model.StudentUserName = "UYIOSA" + "-" + GetRandomAlphanumericString(7);
                    model.StudentPassword = GetRandomAlphanumericString(10);
                    model.StudentSchoolName = SchoolName;
                    model.StudentStatus = "Pending";
                    model.StudentFirstName = "Pending";
                    model.StudentLastName = "Pending";
                    model.StudentMatNo = "Pending";
                    model.StudentPhoneNo = "Pending";
                    model.StudentSex = Gender.Others;

                    db.Student.Add(model);
                }

                    receiptAmount = details.NumberOfStudents * registrationFee;
                    details.RegistrationAmount = receiptAmount;

                    details.SchoolRegistrationStatus = "In Progress";
                    details.SchoolRegisteredBy = test1 + " " + test;
                    
                    db.School.Add(details);
                   

                    // int count = details.NumberOfStudents;

                    //SchoolDetails model1 = new SchoolDetails();
                    DateTime Now = DateTime.Now;
                    String time = Now.ToString();
                    details.SchoolRegisteredTime = time;
                   

                    db.SaveChanges();
                  //  ViewBag.Message = "FECAMDS " + details.SchoolName + " " + "Successfully Registered";
                    ModelState.Clear();
                    return RedirectToAction("ManageSchool");
                }

               catch (Exception ex)
               {

                    throw;
              }
            }
            return View();
        }

        public ActionResult ManageSchool(string searchTerm = null)
        {
            //  var model = (from r in db.School
            //               select r).ToList();
            // return View(model);

            var model =
            db.School
           .Where(r => searchTerm == null || r.SchoolName.StartsWith(searchTerm))
            .OrderByDescending(r => r.SchoolRegisteredTime)
            .Take(10);

                if (Request.IsAjaxRequest()) { 
                    return PartialView("_manageSchoolPartial", model);
               }

            else {
                return View(model);
            }

            
        }

        //Adding more student after initial registration

        public ActionResult RegisterMoreStudents()
        {
            return View();
        }

        //avalable for only super Admin
        public ActionResult RemoveSchool(int id)
        {
            var query = (from r in db.School
                         where r.SchoolId == id
                         select r).SingleOrDefault();

            string schoolName = query.SchoolName;
            db.School.Remove(query);

            var model = (from r in db.Student
                         where r.StudentSchoolName == schoolName
                         select r).ToList();

            foreach (var item in model) {
                db.Student.Remove(item);
            }
            
            db.SaveChanges();
            return RedirectToAction("ManageSchool");
           
        }

        [HttpPost]
        public ActionResult RegisterMoreStudents(SchoolDetails details)
        {
            var query = (from r in db.School
                         where r.SchoolName == details.SchoolName
                         select r).SingleOrDefault();

            if (query != null)
            {
                try
                {
                    query.NumberOfStudents = query.NumberOfStudents + details.NumberOfStudents;
                    // String test1 = Session["MatNo"].ToString();
                    String test = Session["FirstName"].ToString();
                    String test1 = Session["LastName"].ToString();
                    String SchoolName = details.SchoolName;

                    for (int n = 0; n < details.NumberOfStudents; n++)
                    {

                        StudentDetails model = new StudentDetails();

                        model.StudentSchoolName = SchoolName;
                        model.StudentUserName = "UYIOSA" + "-" + GetRandomAlphanumericString(7);
                        model.StudentPassword = GetRandomAlphanumericString(10);
                        model.StudentSchoolName = SchoolName;
                        model.StudentStatus = "Pending";
                        model.StudentFirstName = "Pending";
                        model.StudentLastName = "Pending";
                        model.StudentMatNo = "Pending";
                        model.StudentPhoneNo = "Pending";
                        model.StudentSex = Gender.Others;

                        db.Student.Add(model);
                    }

                    //  query.NumberOfStudents = query.NumberOfStudents + details.NumberOfStudents;

                    receiptAmount = details.NumberOfStudents * registrationFee;

                    query.RegistrationAmount = query.RegistrationAmount + receiptAmount;
                    query.SchoolRegistrationStatus = "In Progress";

                    // int count = details.NumberOfStudents;

                    db.SaveChanges();
                    //  ViewBag.Message = "FECAMDS " + details.SchoolName + " " + "Successfully Registered";
                    ModelState.Clear();
                    return RedirectToAction("ManageSchool");
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                ModelState.Clear();
                return RedirectToAction("RegisterSchool");
            }

            }

        public ActionResult PrintGroupIDcard()
        {
            return View();
        }

        public ActionResult PrintGroupLoginSlip()
        {
            return View();
        }

        //Receipt

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
                return new Rotativa.ActionAsPdf("GeneratePDF", new { mid = id }) { FileName = PdfName, CustomSwitches = footer };
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

        //School Login Slip

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
                return new Rotativa.ActionAsPdf("GeneratePDF2", new { mid = id }) { FileName = PdfName, CustomSwitches = footer };
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
                ViewBag.Title = SchoolName + " Login Slip";

                var model = (from r in db.Student
                             where r.StudentSchoolName == SchoolName && r.StudentStatus== "Pending"
                             select r).ToList();

                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //School ID cards
        public ActionResult GenerateSchoolIDCards(int id)
        {
            try
            {
                var query = (from r in db.School
                             where r.SchoolId == id
                             select r).SingleOrDefault();

               String name = query.SchoolName.ToString();


                string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";

                String PdfName = name + "_ID_Cards.pdf";
                //will take ActionMethod and generate the pdf
                return new Rotativa.ActionAsPdf("GeneratePDF3", new { name2 = name }) { FileName = PdfName, CustomSwitches = footer };
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }
       
        public ActionResult GeneratePDF3(string name2)
        {
            try
            {
                // var model = new GeneratePDFModel();
                //obtain rgistered students associated with a school

                var query = (from s in db.Student
                            join f in db.Files on s.StudentId equals f.StudentId
                            where s.StudentSchoolName == name2
                            select new IDViewModel{ Student = s,
                                                    File = f
                                                 }).ToList();

                //    var query = (from r in db.Student
                //                 where r.StudentSchoolName == name2 && r.StudentStatus == "Registered"
                //                 select r).ToList();

                return View(query);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //managing students

        public ActionResult ViewRegisteredStudents(string searchTerm = null)
        {
            // var model = (from r in db.Student
            //              where r.StudentStatus == "Registered"
            //              orderby r.StudentSchoolName ascending
            //              select r).ToList();
            // return View(model);
            if (searchTerm == null) {
                var model =
               db.Student
              .Where(r => r.StudentStatus == "Registered")
               .OrderByDescending(r => r.StudentRegisteredTime)
               .Take(10);

                return View(model);
            }

           
            else {
                var model =
                db.Student 
               .Where(r => r.StudentMatNo.StartsWith(searchTerm))
                .OrderBy(r => r.StudentSchoolName)
                .Take(10);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_manageStudentPartial", model);
                }

                else {
                    return View(model);
                }
                
            }
            
        }

        public ActionResult RegisteredStudentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StudentDetails student = db.Student.Include(s => s.Files).SingleOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult EditRegisteredStudents(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentDetails student = db.Student.Find(id);

            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRegisteredStudents(StudentDetails details)
        {
        try {
                if (ModelState.IsValid)
                {
                    var query = db.Student.FirstOrDefault(r => r.StudentId == details.StudentId);

                        query.StudentFirstName = details.StudentFirstName;
                        query.StudentLastName = details.StudentLastName;
                        query.StudentMatNo = details.StudentMatNo;
                        query.StudentPhoneNo = details.StudentPhoneNo;
                        query.StudentSex = details.StudentSex;
                        query.StudentSpace = details.StudentSpace;

                        //Commit to the database!

                       db.SaveChanges();
                        return RedirectToAction("ViewRegisteredStudents");
                    }

                return View(details);
            }

            catch (Exception ex)
            {
                throw;
            }

        }

        //Issues with photo
        public ActionResult UnRegisterStudent(int id)
        {
            return View();
        }

        public ActionResult PrintOneIDcard(int id)
        {
            return View();
        }

        public ActionResult PrintOneLoginSlip(int id)
        {
            return View();
        }



        //for SuperAdmin managing Admin

        public ActionResult RegisterAdmin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(AdminDetails details)
        {
            try
            {
                var query = (from r in db.Admin
                             where r.MatNo == details.MatNo 
                             select r).SingleOrDefault();

                if (query != null)
                {
                    ViewBag.Message = "Admin already Registered!";
                    ModelState.Clear();
                    return View();
                }

                else
                {
                    if (ModelState.IsValid)
                    {
                        details.Role = "Pending";
                        details.Status = "Inactive";
                        db.Admin.Add(details);
                        db.SaveChanges();
                        ViewBag.Message = "Admin " + details.FirstName + " " + "Successfully Registered";

                        DateTime Now = DateTime.Now;
                        String time = Now.ToString();
                        details.AdminRegistrationTime = time;

                        ModelState.Clear();
                        return RedirectToAction("ManageAdmin");
                    }

                }
                return View();
            }

            catch (Exception e)
            {
                e.GetBaseException();
                return View("Error");
            }
        }


        public ActionResult ManageAdmin()
        {
            var model = (from r in db.Admin
                         where r.Id != 1
                         orderby r.AdminRegistrationTime descending
                         select r).ToList();
            return View(model);
        }

        public ActionResult RemoveAdmin(int id)
        {
            var query = (from r in db.Admin
                         where r.Id == id
                        select r).SingleOrDefault();

            db.Admin.Remove(query);
            db.SaveChanges();
            return RedirectToAction("ManageAdmin");
        }

        //password and username Generator

        public static string GetRandomAlphanumericString(int length)
        {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";
            return GetRandomString(length, alphanumericCharacters);
        }

        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
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