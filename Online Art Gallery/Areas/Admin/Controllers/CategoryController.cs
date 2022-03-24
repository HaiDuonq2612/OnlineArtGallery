using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Category
        public ActionResult Index()
        {
            ViewData["categorys"] = entities.Categories.ToList();
            return View();

        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Category/Create
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
                TempData["picture-validation"] = "Please Enter Picture";
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
                if (!Directory.Exists(Server.MapPath("~/Content/Images/Category")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Category"));
                    picture.SaveAs(Server.MapPath("~/Content/Images/Category/" + filename));
                }
                else
                {
                    picture.SaveAs(Server.MapPath("~/Content/Images/Category/" + filename));
                }
            }
            else
            {
                TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                return RedirectToAction("Create");
            }

            var check_name = entities.Categories.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Category Name already exists..!";
                return RedirectToAction("Create");

            }

            //Add
            try
            {
                Category category = new Category();
                category.Name = name;
                category.Picture = filename;
                category.Descreption = descreption;
                category.Status = status;

                entities.Categories.Add(category);
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

        // GET: Admin/Category/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var category = entities.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            
            ViewBag.Category = category;
            return View();
        }
        // POST: Admin/Category/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase picture, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Update", new { id = id });
            }
            if (picture == null)
            {
                TempData["picture-validation"] = "Please Enter Picture";
                return RedirectToAction("Update", new { id = id });
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Update", new { id = id });
            }

            //Check Image
            var filename = picture == null ? "" : picture.FileName;
            if (filename != "")
            {
                if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
                {
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/Category")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/Category"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/Category/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/Category/" + filename));
                    }
                }
                else
                {
                    TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                    return RedirectToAction("Update", new { id = id });
                }
            }

            //Check Name
            var check_name = entities.Categories.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Categories Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Update
            try
            {
                var category = entities.Categories.Find(id);
                category.Name = name;
                category.Picture = filename;
                category.Descreption = descreption;
                category.Status = status;

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

        // GET: Admin/Category/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var category = entities.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            ViewData["categorys"] = entities.Categories.ToList();
            ViewBag.Category = category;
            return View();
        }

        // GET: Admin/Category/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var category = entities.Categories.Find(id);
            if (category == null)
            {
                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");
            }

            var artwork = entities.Artworks.FirstOrDefault(s => s.Id_Category == category.Id);
            if (artwork != null)
            {
                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");
            }

            entities.Categories.Remove(category);
            entities.SaveChanges();
            TempData["Success"] = "Delete Success !";
            return RedirectToAction("Index");
        }
    }
}