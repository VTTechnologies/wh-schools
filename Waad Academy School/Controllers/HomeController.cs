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
            if (loginviewmodel.Email == "admin@admin.com" && loginviewmodel.Password == "123")
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
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Student No"),
                                            new DataColumn("School ID"),
                                            new DataColumn("Latitude"),
                                            new DataColumn("Longitude") });


            var ss = _contextdb.Students.Where(s => s.SchoolId == schlid).ToList();

            foreach (var student in ss)
            {
                dt.Rows.Add(student.StudentNo, student.SchoolId, student.Latitude, student.Longitude);
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
    }
}