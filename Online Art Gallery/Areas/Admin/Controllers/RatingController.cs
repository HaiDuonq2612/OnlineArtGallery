using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class RatingController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Rating
        public ActionResult Index()
        {
            ViewData["ratings"] = entities.Ratings.ToList();
            return View();
        }

        // GET: Admin/Rating/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var rating = entities.Ratings.Find(id);
            if (rating == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.Rating = rating;
            return View();
        }
    }
}