
using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class ArtistController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Artist
        public ActionResult Index()
        {
            ViewData["artists"] = entities.Artists.ToList();
            return View();
        }
        // GET: Admin/Artist/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Artist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, HttpPostedFileBase picture, DateTime? birth_date, DateTime? death_date, string birth_place, string style, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return View();
            }
            if (picture == null)
            {
                TempData["picture-validation"] = "Please Enter Picture..!";
                return View();
            }
            if (birth_date == null)
            {
                TempData["birth_date-validation"] = "Please Enter Birth Date..!";
                return View();
            }
            if (birth_place == "")
            {
                TempData["birth_place-validation"] = "Please Enter Birth Place..!";
                return View();
            }
            if (style == "")
            {
                TempData["style-validation"] = "Please Enter Style..!";
                return View();
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return View();
            }

            //Check Image
            var filename = picture.FileName;
            if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
            {
                if (!Directory.Exists(Server.MapPath("~/Content/Images/Artist")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Artist"));
                    picture.SaveAs(Server.MapPath("~/Content/Images/Artist/" + filename));
                }
                else
                {
                    picture.SaveAs(Server.MapPath("~/Content/Images/Artist/" + filename));
                }
            }
            else
            {
                TempData["picture-validation"] = "Photos must be of the correct type: jpg, png..!";
                return RedirectToAction("Create");
            }

            //Check Name
            var check_name = entities.Artists.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Artist Name already exists..!";
                return RedirectToAction("Create");
            }

            //Check Date
            if (death_date != null)
            {
                DateTime date_start = Convert.ToDateTime(birth_date);
                DateTime date_end = Convert.ToDateTime(death_date);
                TimeSpan Time = date_end.Subtract(date_start);
                double days = Time.TotalDays;
                if (days <= 0)
                {
                    TempData["death_date-validation"] = "Create Failed ! DeathDate must be greater than BirthDdate..!";
                    return RedirectToAction("Create");
                }
            }

            //Add
            try
            {
                Artist artist = new Artist();

                artist.Name = name;
                artist.Picture = filename;
                artist.Birth_Date = birth_date;
                artist.Death_Date = death_date;
                artist.Birth_Place = birth_place;
                artist.Style = style;
                artist.Descreption = descreption;
                artist.Status = status;

                entities.Artists.Add(artist);
                entities.SaveChanges();

                TempData["Success"] = "Create Success..!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Create Failed..!";
                return RedirectToAction("Create");
            }
        }

        // GET: Admin/Artist/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var art = entities.Artists.Find(id);
            if (art == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Artist = art;
            return View();
        }
        // POST: Admin/Artist/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase picture, DateTime? birth_date, DateTime? death_date, string birth_place, string style, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (birth_date == null)
            {
                TempData["birth_date-validation"] = "Please Enter Birth Date..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (birth_place == "")
            {
                TempData["birth_place-validation"] = "Please Enter Birth Place..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (style == "")
            {
                TempData["style-validation"] = "Please Enter Style..!";
                return RedirectToAction("Update", new { Id = id });
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
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/Artist")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/Artist"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/Artist/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/Artist/" + filename));
                    }
                }
                else
                {
                    TempData["picture-validation"] = "Photos must be of the correct type: jpg, png..!";
                    return RedirectToAction("Update", new { Id = id });
                }
            }


            Artist artist = entities.Artists.Find(id);

            //Check Name
            var check_name = entities.Artists.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Artist Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Check Date
            if (death_date != null)
            {
                DateTime date_start = Convert.ToDateTime(birth_date);
                DateTime date_end = Convert.ToDateTime(death_date);
                TimeSpan Time = date_end.Subtract(date_start);
                int days = Time.Days;
                if (days <= 0)
                {
                    TempData["death_date-validation"] = "Update Failed ! DeathDate must be greater than BirthDdate..!";
                    return RedirectToAction("Update", new { Id = id });
                }
            }

            //UpDate
            try
            {
                artist.Name = name;
                if (filename != "")
                {
                    artist.Picture = filename;
                }
                artist.Birth_Date = birth_date;
                artist.Death_Date = death_date;
                artist.Birth_Place = birth_place;
                artist.Style = style;
                artist.Descreption = descreption;
                artist.Status = status;

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

        // GET: Admin/Artist/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var art = entities.Artists.Find(id);
            if (art == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Artist = art;
            return View();
        }

        // GET: Admin/Artist/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var artist = entities.Artists.Find(id);

            var artwork = entities.Artworks.FirstOrDefault(s => s.Id_Artist == artist.Id);

            if (artist == null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }
            if (artwork != null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }

            entities.Artists.Remove(artist);
            entities.SaveChanges();
            TempData["Success"] = "Delete Success !";
            return RedirectToAction("Index");
        }
    }
}