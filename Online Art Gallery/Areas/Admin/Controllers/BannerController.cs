using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class BannerController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Banner
        public ActionResult Index()
        {
            ViewData["banners"] = entities.Banners.ToList();
            return View();
        }
        // GET: Admin/Banner/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, HttpPostedFileBase picture, string descreption, bool status)
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
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Create");
            }

            //Check Image
            var filename = picture == null ? "" : picture.FileName;
            if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
            {
                if (!Directory.Exists(Server.MapPath("~/Content/Images/Banner")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Banner"));
                    picture.SaveAs(Server.MapPath("~/Content/Images/Banner/" + filename));
                }
                else
                {
                    picture.SaveAs(Server.MapPath("~/Content/Images/Banner/" + filename));
                }
            }
            else
            {
                TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                return RedirectToAction("Create");
            }

            //Check Image
            var check_name = entities.Banners.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Banner Name already exists..!";
                return RedirectToAction("Create");
            }

            //Add
            try
            {
                Banner banner = new Banner();
                banner.Name = name;
                banner.Picture = filename;
                banner.Descreption = descreption;
                banner.Status = status;

                entities.Banners.Add(banner);
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

        // GET: Admin/Banner/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var banner = entities.Banners.Find(id);
            if (banner == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Banner = banner;
            return View();
        }
        // POST: Admin/Banner/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Banner validation, int id, string name, HttpPostedFileBase picture, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
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
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/Banner")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/Banner"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/Banner/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/Banner/" + filename));
                    }
                }
                else
                {
                    TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                    return RedirectToAction("Update", new { Id = id });
                }
            }

            //Check Name
            var check_name = entities.Banners.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Banner Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }
            //Update
            try
            {
                var banner = entities.Banners.Find(id);
                banner.Name = name;
                if (filename != "")
                {
                    banner.Picture = filename;
                }
                banner.Descreption = descreption;
                banner.Status = status;

                entities.SaveChanges();

                TempData["Success"] = "Update Success..!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
            TempData["Success"] = "Update Success..!";
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/Banner/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var banner = entities.Banners.Find(id);
            if (banner == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Banner = banner;
            return View();
        }

        // GET: Admin/Banner/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var banner = entities.Banners.Find(id);
            if (banner == null)
            {
                TempData["Error"] = "Delete Success..!";
                return RedirectToAction("Index");
            }

            entities.Banners.Remove(banner);
            entities.SaveChanges();
            TempData["Success"] = "Delete Success..!";
            return RedirectToAction("Index");
        }
    }
}