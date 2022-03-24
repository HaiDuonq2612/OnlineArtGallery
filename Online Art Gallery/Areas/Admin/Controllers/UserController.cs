using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/User
        public ActionResult Index()
        {
            ViewData["users"] = entities.Users.Where(x => x.Status == false).ToList();
            return View();
        }

        // GET: Admin/User/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var user = entities.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.User = user;
            return View();
        }
    }
}