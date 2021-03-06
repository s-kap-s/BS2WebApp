﻿using BS2WebApp.Models;
using BS2WebApp.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS2WebApp.Controllers
{
    public class BS2DBController : Controller
    {
        public ActionResult Students()
        {
            using (BS2DBDataContext dbCtx = new BS2DBDataContext())
            {
                List<StudentModel> studentModels = new List<StudentModel>();

                foreach (Student student in dbCtx.Students)
                {
                    studentModels.Add(new StudentModel { Id = student.Id, Name = student.Name });
                }
                return View(studentModels);
            }
        }
 
        public ActionResult EditStudent(int Id)
        {
            if (Id == 0)
            {
                return View(new StudentModel());
            }

            using (BS2DBDataContext dbCtx = new BS2DBDataContext())
            {
                Student student = dbCtx.Students.Where(x => x.Id == Id).FirstOrDefault();

                if (student == null)
                    return View(new StudentModel());
                
                    StudentModel student1 = new StudentModel
                    {
                        Id = student.Id,
                        Name = student.Name
                    };
                return View(student1);
            }
        }
        

        [HttpPost]
        public ActionResult EditStudent(StudentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (BS2DBDataContext dbCtx = new BS2DBDataContext())
                    {
                        if (model.Id == 0)
                        {
                            Student dbStudent = new Student { Name = model.Name };

                            dbCtx.Students.InsertOnSubmit(dbStudent);

                            dbCtx.SubmitChanges();

                            return RedirectToAction("Students");
                        }

                        Student student = dbCtx.Students.Where(x => x.Id == model.Id).FirstOrDefault();

                        if (student == null)
                            RedirectToAction("Error", "Error");

                        student.Name = model.Name;

                        dbCtx.SubmitChanges();

                        return RedirectToAction("Students");
                    }
                }
                return View(model);
            }
            catch
            {
                return RedirectToAction("Error", "Error");
            }
        }

        public ActionResult DeleteStudent(int id)
        {
            using (BS2DBDataContext dbCtx = new BS2DBDataContext())
            {
                Student student = dbCtx.Students.Where(x => x.Id == id).FirstOrDefault();

                if (student == null)
                {
                    return RedirectToAction("Error", "Error");
                }

                dbCtx.Students.DeleteOnSubmit(student);
                dbCtx.SubmitChanges();

                return RedirectToAction("Students");
            }
        }

    }
}