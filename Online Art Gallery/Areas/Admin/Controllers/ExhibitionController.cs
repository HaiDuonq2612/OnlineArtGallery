using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class ExhibitionController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Exhibition
        public ActionResult Index()
        {
            ViewData["exhibitions"] = entities.Exhibitions.ToList();
            return View();
        }

        // GET: Admin/Exhibition/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Exhibition/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create( string name, HttpPostedFileBase picture, DateTime? start_date, DateTime? end_date, float? starting_price, string address, string detail, string descreption)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Create");
            }
            if (picture == null)
            {
                TempData["picture-validation"] = "Please Enter Picture..!";
                return RedirectToAction("Create");
            }
            if (start_date == null)
            {
                TempData["start_date-validation"] = "Please Enter Start Date..!";
                return RedirectToAction("Create");
            }
            if (end_date == null)
            {
                TempData["end_date-validation"] = "Please Enter End Date..!";
                return RedirectToAction("Create");
            }
            if (starting_price == null)
            {
                TempData["starting_price-validation"] = "Please Enter Starting Price..!";
                return RedirectToAction("Create");
            }
            if (address == null)
            {
                TempData["address-validation"] = "Please Enter Address..!";
                return RedirectToAction("Create");
            }
            if (detail == "")
            {
                TempData["detail-validation"] = "Please Enter Detail..!";
                return RedirectToAction("Create");
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Create");
            }

            //Check Image
            var filename = picture == null ? "" : picture.FileName;
            if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
            {
                if (!Directory.Exists(Server.MapPath("~/Content/Images/Exhibition")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Exhibition"));
                    picture.SaveAs(Server.MapPath("~/Content/Images/Exhibition/" + filename));
                }
                else
                {
                    picture.SaveAs(Server.MapPath("~/Content/Images/Exhibition/" + filename));
                }
            }
            else
            {
                TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                return RedirectToAction("Create");
            }
            
            //Check Name
            var check_name = entities.Exhibitions.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Exhibition Name already exists";
                return RedirectToAction("Create");
            }

            //Check Date
            DateTime date_now = DateTime.Now;
            DateTime date_start = Convert.ToDateTime(start_date);
            DateTime date_end = Convert.ToDateTime(end_date);
            TimeSpan s_Time = date_start.Subtract(date_now);
            TimeSpan e_Time = date_end.Subtract(date_start);
            double s_day = s_Time.TotalDays;
            double e_day = e_Time.TotalDays;
            if (s_day <= 0)
            {
                TempData["start_date-validation"] = "Start_Date must be greater than or equal to Date Now..!";
                return RedirectToAction("Create");
            }
            if (e_day <= 0)
            {
                TempData["end_date-validation"] = "End_Date must be greater than Start_Date..!";
                return RedirectToAction("Create");
            }

            //Add
            try
            {
                var Id_admin = Session["Id_Admin"];
                Exhibition exhibition = new Exhibition();
                exhibition.Name = name;
                exhibition.Picture = filename;
                exhibition.Start_Date = start_date;
                exhibition.End_Date = end_date;
                exhibition.Starting_Price = starting_price;
                exhibition.Address = address;
                exhibition.Owner = int.Parse(Id_admin.ToString());
                exhibition.Detail = detail;
                exhibition.Descreption = descreption;
                exhibition.Status = false;

                entities.Exhibitions.Add(exhibition);
                entities.SaveChanges();

                TempData["Success"] = "Create Success..!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Create Failed..!";
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Exhibition/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var ex = entities.Exhibitions.Find(id);
            if (ex == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Exhibition = ex;
            return View();
        }
        // POST: Admin/Exhibition/Update
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase picture, DateTime? start_date, DateTime? end_date, float? starting_price, string address, string detail, string descreption, bool? status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (start_date == null)
            {
                TempData["start_date-validation"] = "Please Enter Start Date..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (end_date == null)
            {
                TempData["end_date-validation"] = "Please Enter End Date..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (starting_price == null)
            {
                TempData["starting_price-validation"] = "Please Enter Starting Price..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (address == null)
            {
                TempData["address-validation"] = "Please Enter Address..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (detail == "")
            {
                TempData["detail-validation"] = "Please Enter Detail..!";
                return RedirectToAction("Create");
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Check Image
            var filename = picture == null ? "" : picture.FileName;
            if (filename != "")
            {
                if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
                {
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/Exhibition")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/Exhibition"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/Exhibition/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/Exhibition/" + filename));
                    }
                }
                else
                {
                    TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                    return RedirectToAction("Update", new { Id = id });
                }
            }

            //Check Name
            var check_name = entities.Exhibitions.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Exhibition Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Check Date
            DateTime date_now = DateTime.Now;
            DateTime date_start = Convert.ToDateTime(start_date);
            DateTime date_end = Convert.ToDateTime(end_date);
            TimeSpan s_Time = date_start.Subtract(date_now);
            TimeSpan e_Time = date_end.Subtract(date_start);
            double s_day = s_Time.TotalDays;
            double e_day = e_Time.TotalDays;
            if (s_day <= 0)
            {
                TempData["start_date-validation"] = "Start_Date must be greater than or equal to Date Now..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (e_day <= 0)
            {
                TempData["end_date-validation"] = "End_Date must be greater than Start_Date..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Update
            try
            {
                var exhibition = entities.Exhibitions.Find(id);

                var Id_admin = Session["Id_Admin"];
                exhibition.Name = name;
                if (filename != "")
                {
                    exhibition.Picture = filename;
                }
                exhibition.Start_Date = start_date;
                exhibition.End_Date = end_date;
                exhibition.Starting_Price = starting_price;
                exhibition.Address = address;
                exhibition.Detail = detail;
                exhibition.Descreption = descreption;
                if (status != null)
                {
                    exhibition.Status = status;
                }
                else
                {
                    exhibition.Status = false;
                }

                entities.SaveChanges();

                TempData["Success"] = "Update Success..!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Update Failed..!";
                return RedirectToAction("Index");
            }

        }

        // GET: Admin/Exhibition/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var ex = entities.Exhibitions.Find(id);
            if (ex == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Exhibition = ex;
            ViewBag.User = entities.Users.Find(ex.Owner);
            return View();
        }

        // GET: Admin/Exhibition/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var exhibition = entities.Exhibitions.Find(id);
            if (exhibition == null)
            {
                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");
            }

            var order = entities.OrderExhibitions.FirstOrDefault(s => s.Id_Exhibition == exhibition.Id);
            if (order != null)
            {
                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");
            }

            try
            {
                entities.Exhibitions.Remove(exhibition);

                entities.SaveChanges();

                TempData["Success"] = "Delete Success !";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");
            }
        }
    }
}