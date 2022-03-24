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
    public class BlogController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Blog
        public ActionResult Index()
        {
            ViewData["blogs"] = entities.Blogs.ToList();
            return View();
        }

        // GET: Admin/Blog/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Blog/Create
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, HttpPostedFileBase picture, string detail, string descreption, bool status)
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
                if (!Directory.Exists(Server.MapPath("~/Content/Images/Blog")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Blog"));
                    picture.SaveAs(Server.MapPath("~/Content/Images/Blog/" + filename));
                }
                else
                {
                    picture.SaveAs(Server.MapPath("~/Content/Images/Blog/" + filename));
                }
            }
            else
            {
                TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                return RedirectToAction("Create");
            }

            //Check Name
            var check_name = entities.Blogs.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Blog Name already exists..!";
                return RedirectToAction("Create");
            }

            //Add
            try
            {
                Blog blog = new Blog();
                blog.Name = name;
                blog.Picture = filename;
                blog.Detail = detail;
                DateTime date = DateTime.Now;
                blog.Date = date;
                blog.Descreption = descreption;
                blog.Status = status;

                entities.Blogs.Add(blog);
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

        // GET: Admin/Blog/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var blog = entities.Blogs.Find(id);
            if (blog == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Blog = blog;
            return View();
        }
        // POST: Admin/Blog/Update
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, HttpPostedFileBase picture, string detail, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (detail == "")
            {
                TempData["detail-validation"] = "Please Enter Detail..!";
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
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/Blog")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/Blog"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/Blog/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/Blog/" + filename));
                    }
                }
                else
                {
                    TempData["ErrorImage"] = "Photos must be of the correct type: jpg, png";
                    return RedirectToAction("Update", new { Id = id });
                }
            }

            //Check Name
            var check_name = entities.Blogs.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Blog Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Update
            try
            {
                var blog = entities.Blogs.Find(id);
                blog.Name = name;
                if (filename != "")
                {
                    blog.Picture = filename;
                }
                blog.Detail = detail;
                blog.Descreption = descreption;
                blog.Status = status;

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

        // GET: Admin/Blog/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var blog = entities.Blogs.Find(id);
            if (blog == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Blog = blog;
            return View();
        }

        // GET: Admin/Blog/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var blog = entities.Blogs.Find(id);
            if (blog == null)
            {
                TempData["Error"] = "Delete Failed..!";
                return RedirectToAction("Index");
            }

            entities.Blogs.Remove(blog);

            entities.SaveChanges();

            TempData["Success"] = "Delete Success..!";
            return RedirectToAction("Index");
        }
    }
}