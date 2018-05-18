using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UYIOSA.Models;

namespace UYIOSA.Controllers
{
    public class StudentAccountController : Controller
    {
        uyiosadb db = new uyiosadb();

        // GET: Student
        public ActionResult Index()
        {
            return RedirectToAction("StudentLogin");
        }

        public ActionResult StudentLogin()
        {
           if (Session["failed Registration"] != null)
            {
                ViewBag.Message = "Registration Unsuccessful!";
                Session["failed Registration"] = null;
            }

            if (Session["Successful Registration"] != null)
            {
                ViewBag.Message = "Registration Successful!";
                Session["failed Registration"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult StudentLogin(StudentDetails details)
        {
           // try
           // {
                var query = (from r in db.Student
                             where r.StudentUserName == details.StudentUserName && r.StudentPassword == details.StudentPassword
                             select r).SingleOrDefault();

                if (query == null)
                {
                    ViewBag.Message = "Invalid UserName or Password!";
                    ModelState.Clear();
                    return View();
                }

                if (query.StudentStatus == "Registered")
                {
                    ViewBag.Message = "Student Registered Already!";
                    ModelState.Clear();
                    return View();
                }

                else
                {
                    Session["StudentUserName"] = query.StudentUserName.ToString();
                    Session["StudentPassword"] = query.StudentPassword.ToString();
                    Session["StudentSchoolName"] = query.StudentSchoolName.ToString();
                    //Session["StudentStatus"] = query.StudentStatus.ToString();

                    return RedirectToAction("StudentLoggedIn");
                }

          //  }

           // catch (Exception e)
           // {
            //    e.GetBaseException();
               // return View("Error");
            //}
        }

        public ActionResult StudentLoggedIn()
        {
            if (Session["StudentUserName"] != null && Session["StudentPassword"] != null )
            {
                    return RedirectToAction("Register", "StudentPage"); 
                //return View();
            }
            else
            {
                return RedirectToAction("StudentLogin");
            }

        }

        public ActionResult StudentLogout()
        {
            Session["StudentUserName"] = null;
            Session["StudentPassword"] = null;

            return RedirectToAction("StudentLogin");
            //return View();
        }


    }
}