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

    public class StudentController : Controller
    {
        private readonly Waad_Academy_SchoolEntities _contextdb;
        public StudentController()
        {
            _contextdb = new Waad_Academy_SchoolEntities();
        }
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WaadAcademy()
        {
            return View();
        }
        public ActionResult Bloom()
        {
            return View();
        }
        public ActionResult AlzikrGirlsSchool()
        {
            return View();
        }
        public ActionResult DarAlThikrBoys()
        {
            return View();
        }
        public ActionResult ThankyouArabic()
        {
            return View();
        }
        public ActionResult Thankyou()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentSave(Student student)
        {
            var std = _contextdb.Students.Where(w => w.StudentNo == student.StudentNo.Trim() && w.SchoolId == student.SchoolId).FirstOrDefault();
            if (std != null)
            {
                std.StudentNo = student.StudentNo;
                std.SchoolId = student.SchoolId;
                std.Latitude = student.Latitude;
                std.Longitude = student.Longitude;
                std.ModifyOn = DateTime.Now;
                _contextdb.SaveChanges();
            }
            else
            {
                student.CreatedOn = DateTime.Now;
                _contextdb.Students.Add(student);
                _contextdb.SaveChanges();
            }
            var response = "Recored Saved";
            return Json(response);
        }
    }
}