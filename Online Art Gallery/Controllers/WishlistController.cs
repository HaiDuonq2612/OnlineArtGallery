using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class WishlistController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Wishlist
        public ActionResult Index()
        {
            int Id_User = (int)Session["Id"];
            ViewData["wishlists"] = entities.Wishlists.Where(x => x.Id_User == Id_User).ToList();
            ViewData["artworks"] = entities.Artworks.ToList();
            return View();
        }

        // Get: Wishlist/Create
        public ActionResult Create(int id)
        {
            Wishlist wishlist = new Wishlist();

            var Id_User = Session["Id"];

            if (Id_User == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var check_wishlist = entities.Wishlists.FirstOrDefault(s => s.Id_User == (int)Id_User && s.Id_Artwork == id);
            if (check_wishlist != null)
            {
                TempData["Error"] = "Artwork already in the Wishlist..!";
                return RedirectToAction("Index");
            }
            wishlist.Id_User = int.Parse(Id_User.ToString());
            wishlist.Id_Artwork = id;

            entities.Wishlists.Add(wishlist);
            entities.SaveChanges();

            TempData["Success"] = "Add your favorite Artwork Successfully..!";

            return RedirectToAction("Index");
        }
        // Get: Wishlist/Delete
        public ActionResult Delete(int id)
        {

            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            
            var wishlist = entities.Wishlists.FirstOrDefault(s => s.Id == id);
            if (wishlist == null)
            {
                TempData["Error"] = "Delete Failed..!";
                return RedirectToAction("Login", "Home");
            }
            entities.Wishlists.Remove(wishlist);
            entities.SaveChanges();

            TempData["Success"] = "Delete Success !";
            return RedirectToAction("Index");

        }
    }
}