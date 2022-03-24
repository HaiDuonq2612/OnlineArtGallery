using Online_Art_Gallery.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class BlogController : Controller
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Blog
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);

            var blogs = (from blog in entities.Blogs select blog).OrderByDescending(x => x.Id);
            return View(blogs.ToPagedList(pageNumber, pageSize));
        }
        // GET: Blog/Detail
        public ActionResult Detail(int id)
        {
            ViewBag.Blog = entities.Blogs.Find(id);

            return View();
        }
    }
}