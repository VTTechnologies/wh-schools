using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Waad_Academy_School.Models;

namespace Waad_Academy_School.Controllers
{
    public class HomeController : Controller
    {
        private readonly Waad_Academy_SchoolEntities _contextdb;

        public HomeController()
        {
            _contextdb = new Waad_Academy_SchoolEntities();
        }

        public ActionResult login()
        {
            Session.Clear();
            return View();
        }
        [HttpPost]
        public ActionResult login(LoginModel loginviewmodel)
        {
            var adminData = _contextdb.UserDetails.Where(x => x.UserName == loginviewmodel.Email && x.Password == loginviewmodel.Password && x.isactive == true).FirstOrDefault();
            if (adminData != null)
            {
                Session["std"] = "true";
                return RedirectToAction("export");
            }
            else
            {
                ViewBag.Message = "Invalid UserName and Password";
                Session["std"] = "false";
                return View();
            }

        }

        public ActionResult export()
        {
            if(Session["std"] == null)
            {
                return RedirectToAction("login");
            }
            if (Session["std"] == "true")
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        [HttpGet]
        public FileResult Exporttoexcel(int schlid)
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Student No"),
                                            new DataColumn("School ID"),
                                            new DataColumn("Latitude"),
                                            new DataColumn("Longitude"),
            new DataColumn("Created Date")});


            var ss = _contextdb.Students.Where(s => s.SchoolId == schlid).ToList();

            foreach (var student in ss)
            {
                dt.Rows.Add(student.StudentNo, student.SchoolId, student.Latitude, student.Longitude, student.CreatedOn);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");


                }
            }

        }
        
        public ActionResult Instruction()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(ManageUserViewModel manageUserViewModel)
        {
            if(ModelState.IsValid)
            {
                var adminModel = _contextdb.UserDetails.Where(x => x.Password == manageUserViewModel.OldPassword).FirstOrDefault();
                if (adminModel == null)
                {
                    ModelState.AddModelError(string.Empty, "Incorrect old password");
                    return View();
                }
                else
                {
                    adminModel.Password = manageUserViewModel.NewPassword;
                    _contextdb.SaveChanges();
                    ViewBag.SuccessMsg = "Password changed successfully";
                    return View();
                }
            }
            return View();
        }
    }
}