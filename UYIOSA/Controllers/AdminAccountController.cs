using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UYIOSA.Models;

namespace UYIOSA.Controllers
{
    public class AdminAccountController : Controller
    {
        uyiosadb db = new uyiosadb();

       // GET: AdminAccount
        public ActionResult Index()
        {
            return RedirectToAction("Adminlogin");
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminDetails details)
        {
            try
            {
                var query = (from r in db.Admin
                             where r.MatNo == details.MatNo && (r.Password == details.Password)
                             select r).SingleOrDefault();

                if (query == null)
                {
                    ViewBag.Message = "Invalid Mat No or Password!";
                    ModelState.Clear();
                    return View();
                }

                else
                {
                    String check = query.Role.ToString();
                    if (check == "Pending")
                    {
                        Session["MatNo"] = query.MatNo.ToString();
                        return RedirectToAction("AdminChangePassword");
                    }
                    else
                    {

                        Session["MatNo"] = query.MatNo.ToString();
                        Session["FirstName"] = query.FirstName.ToString();
                        Session["Role"] = query.Role.ToString();
                        Session["LastName"] = query.LastName.ToString();

                        return RedirectToAction("SuperAdminLoggedIn");
                    }

                }

            }

            catch (Exception e)
            {
                e.GetBaseException();
                return View("Error");
            }
        }

        public ActionResult AdminChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminChangePassword(AdminDetails details)
        {
            String MatNo = Session["MatNo"].ToString();
            String NewPassword = details.Password;


            var query = (from r in db.Admin
                         where r.MatNo == MatNo
                         select r).SingleOrDefault();

            if (query != null)
            {
                query.Password = NewPassword;
                query.ConfirmPassword = NewPassword;
                query.Role = "Admin";
                query.Status = "Active";
                db.SaveChanges();
                ViewBag.Message = "Password Successfully Changed!";
                ModelState.Clear();
                return View();
            }
            else
            {
                ModelState.Clear();
                return View();
            }
        }

        public ActionResult AdminLoggedIn()
        {
            return View();
        }

        public ActionResult SuperAdminLoggedIn()
        {
            String Check = Session["Role"].ToString();

            if (Session["MatNo"] != null && Check == "SuperAdmin" )
            {
              //  return RedirectToAction("Index", "SuperAdminAccount", new { area = "" });
               return View();
            }

            else if (Session["MatNo"] != null && Check == "Admin")
            {
                //return RedirectToAction("Index", "AdminAccount", new { area = "" });
              return RedirectToAction("AdminLoggedIn");
            }

            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        public ActionResult AdminLogout()
        {
            Session["MatNo"] = null;
            return RedirectToAction("AdminLogin");
        }

        protected override void Dispose(bool disposing)
        {
            if(db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}